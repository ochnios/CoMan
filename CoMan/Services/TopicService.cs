using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;
using CoMan.Models.AuxiliaryModels;
using CoMan.Extensions;

namespace CoMan.Services
{
    public class TopicService : ITopicService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public TopicService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger logger)
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

            if (await IsCurrentUserAllowedToModify(topic))
            {
                if (await CanBeModified(topic))
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
            if (dtParameters == null)
            {
                throw new ArgumentNullException(nameof(dtParameters));
            }

            var searchParams = dtParameters.Search ?? new DtSearch();
            var searchBy = dtParameters.Search!.Value ?? string.Empty;
            searchBy = searchBy.ToUpper();

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;
            var includeArchived = false;
            var onlyMine = false;

            if (dtParameters.Order != null)
            {
                // sort on the 1st column
                orderCriteria = dtParameters.Columns![dtParameters.Order[0].Column].Data ?? string.Empty;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            if (dtParameters.IncludeArchived != null)
            {
                includeArchived = dtParameters.IncludeArchived == "true";
            }

            if (dtParameters.OnlyMine != null)
            {
                onlyMine = dtParameters.OnlyMine == "true";
            }

             var currentUserId = await _userManager.GetCurrentUserId();

            var rawResults = await _unitOfWork.Topics
                .FindForDatatables((r => 
                            (!onlyMine || r.Author.Id == currentUserId) &&
                            (includeArchived || r.Status != TopicStatus.Archived) && 
                            (
                                r.Title.ToUpper().Contains(searchBy) ||
                                r.Description != null && r.Description.ToUpper().Contains(searchBy) ||
                                r.Author.FirstName.ToUpper().Contains(searchBy) ||
                                r.Author.LastName.ToUpper().Contains(searchBy)
                            )),
                            dtParameters.Start, dtParameters.Length, orderCriteria, orderAscendingDirection
                );

            List<TopicDatatable> resultsForDatatable = new();
            foreach (var item in rawResults.Results)
            {
                resultsForDatatable.Add(new TopicDatatable()
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
            var author = await GetCurrentTeacherUser();

            newTopic.AddedDate = System.DateTime.Now;
            newTopic.Status = TopicStatus.Active;
            newTopic.Author = author;

            await _unitOfWork.Topics.AddAsync(newTopic);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("New topic {topicId} added by {authorId}", newTopic.Id, author.Id);

            return newTopic;
        }

        public async Task UpdateTopic(int id, TopicModel updatedTopic)
        {
            var topicToBeUpdated = await GetTopicForModificationById(id);
            topicToBeUpdated.Title = updatedTopic.Title;
            topicToBeUpdated.Description = updatedTopic.Description;
            topicToBeUpdated.StudentLimit = updatedTopic.StudentLimit;
            topicToBeUpdated.Status = updatedTopic.Status;

            await _unitOfWork.CommitAsync();
        }

        public async Task ArchiveTopic(int id)
        {
            var topicToBeArchived= await GetTopicForModificationById(id);
            topicToBeArchived.Status = TopicStatus.Archived;
            await _unitOfWork.CommitAsync();
        }

        public async Task ActivateTopic(int id)
        {
            var topicToBeArchived = await GetTopicForModificationById(id);
            topicToBeArchived.Status = TopicStatus.Active;
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteTopic(int id)
        {
            var topicToBeDeleted = await GetTopicForModificationById(id);
            _unitOfWork.Topics.Remove(topicToBeDeleted);
            await _unitOfWork.CommitAsync();
        }

        public async Task<int> GetCountOfAcceptedRequests(int id) {
            var topic = await _unitOfWork.Topics.GetByIdAsync(id);
            return topic.CooperationRequests!.Count(c => c.Status == CooperationRequestStatus.Accepted);
        }

        private async Task<Boolean> IsCurrentUserAllowedToModify(TopicModel topic)
        {
            var isAdmin = await _userManager.IsCurrentUserInRole("Admin");
            var currentUserId = await _userManager.GetCurrentUserId();

            return (isAdmin || currentUserId.Equals(topic.Author.Id));
        }

        private async Task<Boolean> CanBeModified(TopicModel topic)
        {
            return !(await HasAnyRelatedEntities(topic));
        }

        private async Task<TeacherUser> GetCurrentTeacherUser()
        {
            var currentUserId = await _userManager.GetCurrentUserId();
            return await _unitOfWork.Teachers.SingleOrDefaultAsync(t => t.Id == currentUserId);
        }

        private async Task<Boolean> HasAnyRelatedEntities(TopicModel topic)
        {
            var topicRepository = _unitOfWork.Topics;
            var foundTopic = await topicRepository.SingleOrDefaultAsync(
                t => t.Id == topic.Id && (t.CooperationRequests!.Any() || t.Cooperations!.Any()));
            return foundTopic != null;
        }
    }
}