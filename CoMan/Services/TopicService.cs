using System.Collections.Generic;
using System.Threading.Tasks;
using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;
using System.Linq.Expressions;
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
            var author = await GetCurrentUser();

            newTopic.AddedDate = System.DateTime.Now;
            newTopic.Status = TopicStatus.Active;
            newTopic.Author = author;

            await _unitOfWork.Topics.AddAsync(newTopic);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("New topic {topicId} added by {authorId}", newTopic.Id, author.Id);

            return newTopic;
        }

        public async Task UpdateTopic(TopicModel TopicToBeUpdated, TopicModel Topic)
        {
            TopicToBeUpdated.Title = Topic.Title;
            TopicToBeUpdated.Description = Topic.Description;
            TopicToBeUpdated.StudentLimit = Topic.StudentLimit;
            TopicToBeUpdated.Status = Topic.Status;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteTopic(TopicModel Topic)
        {
            _unitOfWork.Topics.Remove(Topic);
            await _unitOfWork.CommitAsync();
        }
        private async Task<TeacherUser> GetCurrentUser()
        {
            TeacherService teacherService = new TeacherService(_unitOfWork, _userManager);
            var httpContext = new HttpContextAccessor().HttpContext;
            var currentUser = await _userManager.GetUserAsync(httpContext.User);
            return await teacherService.GetTeacherById(currentUser.Id);
        }
    }
}