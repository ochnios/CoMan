using CoMan.Models;
using System.Security.Cryptography;

namespace CoMan.Services
{
    public interface ITopicService
    {
        Task<TopicModel> GetTopicById(int id);
        Task<IEnumerable<TopicModel>> GetAllTopics();
        Task<IEnumerable<TopicModel>> FindForDatables(string searchBy);
        Task<TopicModel> CreateTopic(TopicModel newTopic);
        Task UpdateTopic(TopicModel TopicToBeUpdated, TopicModel Topic);
        Task DeleteTopic(TopicModel Topic);
    }
}