using System.Collections.Generic;
using System.Threading.Tasks;
using CoMan.Models;
using CoMan.Data;

namespace CoMan.Services
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TopicService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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
            newTopic.AddedDate = System.DateTime.Now;
            newTopic.Status = TopicStatus.Active;
            await _unitOfWork.Topics.AddAsync(newTopic);
            await _unitOfWork.CommitAsync();
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
    }
}