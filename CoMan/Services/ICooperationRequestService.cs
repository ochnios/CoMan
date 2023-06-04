using CoMan.Models;
using CoMan.Models.AuxiliaryModels;

namespace CoMan.Services
{
    public interface ICooperationRequestService
    {
        Task<dynamic> FindDatablesForCurrentUser(DtParameters dtParameters);
        Task<CooperationRequestModel> GetCooperationRequestForCurrentUserById(int id);
        Task<CooperationRequestModel> CreateCooperationRequest(CooperationRequestModel newCooperationRequest, int topicId, string teacherId);
        Task UpdateCooperationRequest(int id, CooperationRequestModel updatedCooperationRequest);
        Task AcceptCooperationRequest(int id);
        Task RejectCooperationRequest(int id, CooperationRequestModel rejectedCooperationRequest);
        Task ArchiveCooperationRequest(int id);
        Task DeleteCooperationRequest(int id);
    }
}