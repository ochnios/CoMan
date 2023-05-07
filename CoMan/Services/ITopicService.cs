using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using System.Security.Cryptography;

namespace CoMan.Services
{
    public interface ITopicService
    {
        Task<TopicModel> GetTopicById(int id);
        Task<IEnumerable<TopicModel>> GetAllTopics();
        Task<IEnumerable<TopicModel>> FindForDatables(DtParameters dtParameters);
        Task<int> CountTopics();
        Task<TopicModel> CreateTopic(TopicModel newTopic);
        Task UpdateTopic(TopicModel TopicToBeUpdated, TopicModel Topic);
        Task DeleteTopic(TopicModel Topic);
    }
}