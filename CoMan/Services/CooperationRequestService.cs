using System.Collections.Generic;
using System.Threading.Tasks;
using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;

namespace CoMan.Services
{
    public class CooperationRequestService : ICooperationRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CooperationRequestService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
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

        public async Task<CooperationRequestModel> CreateCooperationRequest(CooperationRequestModel newCooperationRequest, int topicId, string teacherId)
        {
            var author = await GetCurrentStudentUser();

            newCooperationRequest.Topic = await _unitOfWork.Topics.SingleOrDefaultAsync(t => t.Id == topicId);
            newCooperationRequest.Teacher = await _unitOfWork.Teachers.SingleOrDefaultAsync(t => t.Id == teacherId);
            newCooperationRequest.Student = author;
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

        private async Task<StudentUser> GetCurrentStudentUser()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var currentUser = await _userManager.GetUserAsync(httpContext.User);
            return await _unitOfWork.Students.SingleOrDefaultAsync(t => t.Id == currentUser.Id);
        }
    }
}