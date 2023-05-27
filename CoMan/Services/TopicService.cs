using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;
using CoMan.Models.AuxiliaryModels;

namespace CoMan.Services
{
    public class TopicService : ITopicService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public TopicService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<TopicService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<TopicModel> GetTopicById(int id)
        {
            return await _unitOfWork.Topics
                .GetByIdAsync(id);
        }
        public async Task<TopicModel> GetTopicForModificationById(int id)
        {
            TopicModel topic = await _unitOfWork.Topics
                .GetByIdAsync(id);

            if (await isUserAllowedToModify(topic))
            {
                if (await canBeModified(topic))
                {
                    return topic;
                }
                else
                {
                    throw new Exception("You can not edit the topic which is related to any cooperation or cooperation request!");
                }
            }
            else
            {
                throw new Exception("You are not alllowed to modify others users topics!");
            }
        }


        public async Task<IEnumerable<TopicModel>> GetAllTopics()
        {
            return await _unitOfWork.Topics
                .GetAllAsync();
        }

        public async Task<dynamic> FindForDatables(DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search.Value.ToUpper();

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var rawResults = await _unitOfWork.Topics
                .FindForDatatables((r => r.Title.ToUpper().Contains(searchBy) ||
                           r.Description != null && r.Description.ToUpper().Contains(searchBy) ||
                           r.Author.FirstName.ToUpper().Contains(searchBy) ||
                           r.Author.LastName.ToUpper().Contains(searchBy)),
                           dtParameters.Start, dtParameters.Length, orderCriteria, orderAscendingDirection
                );

            List<TopicTable> resultsForDatatable = new List<TopicTable>();
            foreach (var item in rawResults.Results)
            {
                resultsForDatatable.Add(new TopicTable()
                {
                    Id = item.Id,
                    AddedDate = item.AddedDate.ToString("dd.MM.yyyy"),
                    Status = item.Status.ToString(),
                    Title = item.Title,
                    StudentLimit = item.StudentLimit,
                    AuthorId = item.Author.Id,
                    AuthorName = item.Author.FirstName + " " + item.Author.LastName,
                });
            }

            return new
            {
                ResultsForTable = resultsForDatatable,
                TotalCount = rawResults.TotalCount,
                FilteredCount = rawResults.FilteredCount
            };
        }

        public async Task<TopicModel> CreateTopic(TopicModel newTopic)
        {
            var author = await getCurrentTeacherUser();

            newTopic.AddedDate = System.DateTime.Now;
            newTopic.Status = TopicStatus.Active;
            newTopic.Author = author;

            await _unitOfWork.Topics.AddAsync(newTopic);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("New topic {topicId} added by {authorId}", newTopic.Id, author.Id);

            return newTopic;
        }

        public async Task UpdateTopic(TopicModel topicToBeUpdated, TopicModel updatedTopic)
        {
            topicToBeUpdated.Title = updatedTopic.Title;
            topicToBeUpdated.Description = updatedTopic.Description;
            topicToBeUpdated.StudentLimit = updatedTopic.StudentLimit;
            topicToBeUpdated.Status = updatedTopic.Status;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteTopic(TopicModel topicToBeDeleted)
        {
            _unitOfWork.Topics.Remove(topicToBeDeleted);
            await _unitOfWork.CommitAsync();
        }

        private async Task<Boolean> isUserAllowedToModify(TopicModel topic)
        {
            var currentUserRole = await getCurrentUserRole();
            var currentUserId = await getCurrentUserId();

            return (currentUserRole.Equals("Admin") || currentUserId.Equals(topic.Author.Id));
        }

        private async Task<Boolean> canBeModified(TopicModel topic)
        {
            return !(await hasAnyRelatedEntities(topic));
        }

        private async Task<TeacherUser> getCurrentTeacherUser()
        {
            var currentUserId = await getCurrentUserId();
            return await _unitOfWork.Teachers.SingleOrDefaultAsync(t => t.Id == currentUserId);
        }

        private async Task<string> getCurrentUserId()
        {
            var currentUser = await getCurrentUser();
            return currentUser.Id;
        }

        private async Task<string> getCurrentUserRole()
        {
            var currentUser = await getCurrentUser();
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
            return currentUserRoles.First<string>();
        }

        private async Task<ApplicationUser> getCurrentUser()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            return await _userManager.GetUserAsync(httpContext.User); ;
        }

        private async Task<Boolean> hasAnyRelatedEntities(TopicModel topic)
        {
            var topicRepository = _unitOfWork.Topics;
            var foundTopic = await topicRepository.SingleOrDefaultAsync(
                t => t.Id == topic.Id && (t.CooperationRequests.Any() || t.Cooperations.Any()));
            return foundTopic != null;
        }
    }
}