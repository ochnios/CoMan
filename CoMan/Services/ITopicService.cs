using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using System.Security.Cryptography;

namespace CoMan.Services
{
    public interface ITopicService
    {
        Task<TopicModel> GetTopicById(int id);
        Task<TopicModel> GetTopicForModificationById(int id);
        Task<IEnumerable<TopicModel>> GetAllTopics();
        Task<dynamic> FindForDatables(DtParameters dtParameters);
        Task<TopicModel> CreateTopic(TopicModel newTopic);
        Task UpdateTopic(int id, TopicModel updatedTopic);
        Task ActivateTopic(int id);
        Task ArchiveTopic(int id);
        Task DeleteTopic(int id);
    }
}