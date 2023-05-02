using System.Collections.Generic;
using System.Threading.Tasks;
using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;

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