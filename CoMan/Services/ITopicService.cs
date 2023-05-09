using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using System.Security.Cryptography;

namespace CoMan.Services
{
    public interface ITopicService
    {
        Task<TopicModel> GetTopicById(int id);
        Task<IEnumerable<TopicModel>> GetAllTopics();
        Task<dynamic> FindForDatables(DtParameters dtParameters);
        Task<TopicModel> CreateTopic(TopicModel newTopic);
        Task UpdateTopic(TopicModel TopicToBeUpdated, TopicModel Topic);
        Task DeleteTopic(TopicModel Topic);
    }
}