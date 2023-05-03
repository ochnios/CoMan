using System.Collections.Generic;
using System.Threading.Tasks;
using CoMan.Models;
using CoMan.Data;

namespace CoMan.Services
{
    public class CooperationRequestService : ICooperationRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CooperationRequestService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<CooperationRequestModel> GetCooperationRequestById(int id)
        {
            return await _unitOfWork.CooperationRequests
                .GetByIdAsync(id);
        }

        public async Task<IEnumerable<CooperationRequestModel>> GetAllCooperationRequests()
        {
            return await _unitOfWork.CooperationRequests
                .GetAllAsync();
        }

        public async Task<CooperationRequestModel> CreateCooperationRequest(CooperationRequestModel newCooperationRequest)
        {
            newCooperationRequest.Topic = await _unitOfWork.Topics.SingleOrDefaultAsync(t => t.Id == 1); ;
            newCooperationRequest.CreationDate = System.DateTime.Now;
            newCooperationRequest.Status = CooperationRequestStatus.Waiting;
            await _unitOfWork.CooperationRequests.AddAsync(newCooperationRequest);
            await _unitOfWork.CommitAsync();
            return newCooperationRequest;
        }

        public async Task UpdateCooperationRequest(CooperationRequestModel CooperationRequestToBeUpdated, CooperationRequestModel CooperationRequest)
        {
            CooperationRequestToBeUpdated.ApplicantComment = CooperationRequest.ApplicantComment;
            CooperationRequestToBeUpdated.RecipentComment = CooperationRequest.RecipentComment;
            CooperationRequestToBeUpdated.Status = CooperationRequest.Status;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCooperationRequest(CooperationRequestModel CooperationRequest)
        {
            _unitOfWork.CooperationRequests.Remove(CooperationRequest);
            await _unitOfWork.CommitAsync();
        }
    }
}