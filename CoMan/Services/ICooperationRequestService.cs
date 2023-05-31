using CoMan.Models;
using CoMan.Models.AuxiliaryModels;

namespace CoMan.Services
{
    public interface ICooperationRequestService
    {
        Task<dynamic> FindDatablesForCurrentUser(DtParameters dtParameters);
        Task<CooperationRequestModel> GetCooperationRequestById(int id);
        Task<CooperationRequestModel> CreateCooperationRequest(CooperationRequestModel newCooperationRequest, int topicId, string teacherId);
        Task UpdateCooperationRequest(CooperationRequestModel cooperationRequestToBeUpdated, CooperationRequestModel updatedCooperationRequest);
        Task DeleteCooperationRequest(CooperationRequestModel cooperationRequestToBeDeleted);
    }
}