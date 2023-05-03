using CoMan.Models;

namespace CoMan.Services
{
    public interface ICooperationRequestService
    {
        Task<IEnumerable<CooperationRequestModel>> GetAllCooperationRequests();
        Task<CooperationRequestModel> GetCooperationRequestById(int id);
        Task<CooperationRequestModel> CreateCooperationRequest(CooperationRequestModel newCooperationRequest);
        Task UpdateCooperationRequest(CooperationRequestModel CooperationRequestToBeUpdated, CooperationRequestModel CooperationRequest);
        Task DeleteCooperationRequest(CooperationRequestModel CooperationRequest);
    }
}