using CoMan.Models;

namespace CoMan.Services
{
    public interface ITopicService
    {
        Task<TopicModel> GetTopicById(int id);
        Task<IEnumerable<TopicModel>> GetAllTopics();
        Task<TopicModel> CreateTopic(TopicModel newTopic);
        Task UpdateTopic(TopicModel TopicToBeUpdated, TopicModel Topic);
        Task DeleteTopic(TopicModel Topic);
    }
}