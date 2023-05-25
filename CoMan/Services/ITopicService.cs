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
        Task UpdateTopic(TopicModel topicToBeUpdated, TopicModel updatedTopic);
        Task DeleteTopic(TopicModel topicToBeDeleted);
        Task<Boolean> IsUserAllowedToModifyTopic(TopicModel topic);
    }
}