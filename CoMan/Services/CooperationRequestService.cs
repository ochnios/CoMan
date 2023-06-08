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
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<CooperationRequestModel> GetCooperationRequestForCurrentUserById(int id)
        {
            var cooperationRequest = await _unitOfWork.CooperationRequests.GetByIdAsync(id);
            var currentUserId = await _userManager.GetCurrentUserId();

            if (currentUserId.Equals(cooperationRequest.Student!.Id) || currentUserId.Equals(cooperationRequest.Teacher!.Id))
            {
                return cooperationRequest;
            }
            else
            {
                throw new Exception("You are not allowed to manage this cooperation request!");
            }
        }

        public async Task<dynamic> FindDatablesForCurrentUser(DtParameters dtParameters)
        {
            if (dtParameters == null)
            {
                throw new ArgumentNullException(nameof(dtParameters));
            }

            var searchParams = dtParameters.Search ?? new DtSearch();
            var searchBy = dtParameters.Search!.Value ?? string.Empty;
            searchBy = searchBy.ToUpper();

            // if we have an empty search then just order the results by Id ascending
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // sort on the 1st column
                orderCriteria = dtParameters.Columns![dtParameters.Order[0].Column].Data ?? string.Empty;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var currentUserId = await _userManager.GetCurrentUserId();

            var rawResults = await _unitOfWork.CooperationRequests
                .FindForDatatables((r => (currentUserId.Equals(r.Teacher!.Id) || currentUserId.Equals(r.Student!.Id)) &&
                           (r.Teacher.FirstName.ToUpper().Contains(searchBy) ||
                           r.Teacher.LastName.ToUpper().Contains(searchBy) ||
                           r.Student!.FirstName.ToUpper().Contains(searchBy) ||
                           r.Student!.LastName.ToUpper().Contains(searchBy))),
                           dtParameters.Start, dtParameters.Length, orderCriteria, orderAscendingDirection
                );

            List<CooperationRequestDatatable> resultsForDatatable = new();
            foreach (var item in rawResults.Results)
            {
                resultsForDatatable.Add(new CooperationRequestDatatable()
                {
                    Id = item.Id,
                    CreationDate = item.CreationDate != null ? item.CreationDate.ToString("dd.MM.yyyy") : string.Empty,
                    Status = item.Status != null ? item.Status.ToString() : string.Empty,
                    ConsiderationDate = item.ConsiderationDate != null ? item.ConsiderationDate.ToString("dd.MM.yyyy") : string.Empty,
                    Student = item.Student != null ? item.Student.FirstName + " " + item.Student.LastName : string.Empty,
                    Teacher = item.Teacher != null ? item.Teacher.FirstName + " " + item.Teacher.LastName : string.Empty,
                    Topic = item.Topic != null ? item.Topic.Title : string.Empty
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

        public async Task UpdateCooperationRequest(int id, CooperationRequestModel updatedCooperationRequest)
        {
            var cooperationRequestToBeUpdated = await GetCooperationRequestForCurrentUserById(id);
            if(!CanBeUpdated(cooperationRequestToBeUpdated))
            {
                throw new Exception("Cooperation request cannot be updated!");
            }

            cooperationRequestToBeUpdated.StudentComment = updatedCooperationRequest.StudentComment;
            await _unitOfWork.CommitAsync();
        }

        public async Task AcceptCooperationRequest(int id)
        {
            var cooperationRequestToBeAccepted = await GetCooperationRequestForCurrentUserById(id);
            if(!CanBeAccepted(cooperationRequestToBeAccepted))
            {
                throw new Exception("Cooperation request cannot be accepted!");
            }

            cooperationRequestToBeAccepted.Status = CooperationRequestStatus.Accepted;
            cooperationRequestToBeAccepted.ConsiderationDate = System.DateTime.Now;
            await _unitOfWork.CommitAsync();
        }

        public async Task RejectCooperationRequest(int id, CooperationRequestModel rejectedCooperationRequest)
        {
            var cooperationRequestToBeRejected = await GetCooperationRequestForCurrentUserById(id);
            if(!CanBeRejected(cooperationRequestToBeRejected))
            {
                throw new Exception("Cooperation request cannot be rejected!");
            }

            cooperationRequestToBeRejected.Status = CooperationRequestStatus.Rejected;
            cooperationRequestToBeRejected.ConsiderationDate = System.DateTime.Now;
            cooperationRequestToBeRejected.TeacherComment = rejectedCooperationRequest.TeacherComment;
            await _unitOfWork.CommitAsync();
        }

        public async Task ArchiveCooperationRequest(int id)
        {
            var cooperationRequestToBeArchived = await GetCooperationRequestForCurrentUserById(id);
            if(!CanBeArchived(cooperationRequestToBeArchived))
            {
                throw new Exception("Cooperation request cannot be archived!");
            }

            cooperationRequestToBeArchived.Status = CooperationRequestStatus.Archived;
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCooperationRequest(int id)
        {
            var cooperationRequestToBeDeleted = await GetCooperationRequestForCurrentUserById(id);
            if(!CanBeDeleted(cooperationRequestToBeDeleted))
            {
                throw new Exception("Cooperation request cannot be deleted!");
            }

            _unitOfWork.CooperationRequests.Remove(cooperationRequestToBeDeleted);
            await _unitOfWork.CommitAsync();
        }

        private async Task<Boolean> IsCurrentUserAllowedToModify(CooperationRequestModel cooperationRequest)
        {
            var currentUserId = await _userManager.GetCurrentUserId();
            return (currentUserId.Equals(cooperationRequest.Student!.Id) || currentUserId.Equals(cooperationRequest.Teacher!.Id));
        }

        private async Task<StudentUser> GetCurrentStudentUser()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var currentUser = await _userManager.GetUserAsync(httpContext!.User);
            return await _unitOfWork.Students.SingleOrDefaultAsync(t => t.Id == currentUser.Id);
        }

        private Boolean CanBeUpdated(CooperationRequestModel cooperationRequest)
        {
            return cooperationRequest.Status == CooperationRequestStatus.Waiting;
        }

        private Boolean CanBeDeleted(CooperationRequestModel cooperationRequest)
        {
            return cooperationRequest.Status == CooperationRequestStatus.Waiting;
        }

        private Boolean CanBeArchived(CooperationRequestModel cooperationRequest)
        {
            return cooperationRequest.Status == CooperationRequestStatus.Accepted;
        }

        private Boolean CanBeAccepted(CooperationRequestModel cooperationRequest)
        {
            return cooperationRequest.Status == CooperationRequestStatus.Waiting;
        }

        private Boolean CanBeRejected(CooperationRequestModel cooperationRequest)
        {
            return cooperationRequest.Status == CooperationRequestStatus.Waiting;
        }
    }
}