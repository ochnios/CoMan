using CoMan.Models;
using CoMan.Models.AuxiliaryModels;

namespace CoMan.Services
{
    public interface ICooperationService
    {
        Task<dynamic> FindDatablesForCurrentUser(DtParameters dtParameters);
        Task<CooperationModel> GetCooperationForCurrentUserById(int id);
        Task<CooperationModel> CreateCooperation(CooperationModel newCooperation, int requestId, int topicId, string teacherId);
        Task UpdateCooperation(int id, CooperationModel updatedCooperation);
        Task ArchiveCooperation(int id);
        Task EndCooperation(int id, CooperationModel endedCooperation);
    }
}