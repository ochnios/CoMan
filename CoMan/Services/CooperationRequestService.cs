using System.Collections.Generic;
using System.Threading.Tasks;
using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;
using CoMan.Models.AuxiliaryModels;
using CoMan.Extensions;

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

        public async Task<IEnumerable<CooperationRequestModel>> GetAllCooperationRequestsForCurrentUser()
        {
            return await _unitOfWork.CooperationRequests
                .GetAllAsync();
        }

        public async Task<dynamic> FindDatablesForCurrentUser(DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search.Value.ToUpper();

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var currentUserId = await _userManager.GetCurrentUserId();

            var rawResults = await _unitOfWork.CooperationRequests
                .FindForDatatables((r => (currentUserId.Equals(r.Teacher.Id) || currentUserId.Equals(r.Student.Id)) &&
                           (r.Teacher.FirstName.ToUpper().Contains(searchBy) ||
                           r.Teacher.LastName.ToUpper().Contains(searchBy) ||
                           r.Student.FirstName.ToUpper().Contains(searchBy) ||
                           r.Student.LastName.ToUpper().Contains(searchBy))),
                           dtParameters.Start, dtParameters.Length, orderCriteria, orderAscendingDirection
                );

            List<CooperationRequestDatatable> resultsForDatatable = new List<CooperationRequestDatatable>();
            foreach (var item in rawResults.Results)
            {
                resultsForDatatable.Add(new CooperationRequestDatatable()
                {
                    Id = item.Id,
                    CreationDate = item.CreationDate != null ? item.CreationDate.ToString("dd.MM.yyyy") : string.Empty,
                    Status = item.Status != null ? item.Status.ToString() : string.Empty,
                    ConsiderationDate = item.ConsiderationDate != null ? item.ConsiderationDate.ToString("dd.MM.yyyy") : string.Empty,
                    Student = item.Student != null ? item.Student.FirstName + " " + item.Student.LastName : string.Empty,
                    Teacher = item.Teacher != null ? item.Teacher.FirstName + " " + item.Teacher.LastName : string.Empty
                });
            }

            return new
            {
                ResultsForTable = resultsForDatatable,
                TotalCount = rawResults.TotalCount,
                FilteredCount = rawResults.FilteredCount
            };

        }

        public async Task<CooperationRequestModel> CreateCooperationRequest(CooperationRequestModel newCooperationRequest, int topicId, string teacherId)
        {
            var author = await GetCurrentStudentUser();

            newCooperationRequest.Topic = await _unitOfWork.Topics.SingleOrDefaultAsync(t => t.Id == topicId);
            newCooperationRequest.Teacher = await _unitOfWork.Teachers.SingleOrDefaultAsync(t => t.Id.Equals(teacherId));
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