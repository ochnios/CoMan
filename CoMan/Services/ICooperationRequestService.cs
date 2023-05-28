using CoMan.Models;
using CoMan.Models.AuxiliaryModels;

namespace CoMan.Services
{
    public interface ICooperationRequestService
    {
        Task<IEnumerable<CooperationRequestModel>> GetAllCooperationRequestsForCurrentUser();
        Task<dynamic> FindDatablesForCurrentUser(DtParameters dtParameters);
        Task<CooperationRequestModel> GetCooperationRequestById(int id);
        Task<CooperationRequestModel> CreateCooperationRequest(CooperationRequestModel newCooperationRequest, int topicId, string teacherId);
        Task UpdateCooperationRequest(CooperationRequestModel CooperationRequestToBeUpdated, CooperationRequestModel CooperationRequest);
        Task DeleteCooperationRequest(CooperationRequestModel CooperationRequest);
    }
}