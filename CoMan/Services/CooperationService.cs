using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;
using CoMan.Models.AuxiliaryModels;
using CoMan.Extensions;

namespace CoMan.Services
{
    public class CooperationService : ICooperationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICooperationRequestService _cooperationRequestService;

        public CooperationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ICooperationRequestService cooperationRequestService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cooperationRequestService = cooperationRequestService;
        }

        public async Task<CooperationModel> GetCooperationForCurrentUserById(int id)
        {
            var cooperation = await _unitOfWork.Cooperations.GetByIdAsync(id);
            var currentUserId = await _userManager.GetCurrentUserId();

            if (cooperation != null && (currentUserId.Equals(cooperation.Student!.Id) || currentUserId.Equals(cooperation.Teacher!.Id)))
            {
                return cooperation;
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

            var rawResults = await _unitOfWork.Cooperations
                .FindForDatatables((r => (currentUserId.Equals(r.Teacher!.Id) || currentUserId.Equals(r.Student!.Id)) &&
                           (r.Teacher.FirstName.ToUpper().Contains(searchBy) ||
                           r.Teacher.LastName.ToUpper().Contains(searchBy) ||
                           r.Student!.FirstName.ToUpper().Contains(searchBy) ||
                           r.Student!.LastName.ToUpper().Contains(searchBy) ||
                           r.Topic!.Title.ToUpper().Contains(searchBy)
                           )),
                           dtParameters.Start, dtParameters.Length, orderCriteria, orderAscendingDirection
                );

            List<CooperationDatatable> resultsForDatatable = new();
            foreach (var item in rawResults.Results)
            {
                resultsForDatatable.Add(new CooperationDatatable()
                {
                    Id = item.Id,
                    StartDate = item.CreationDate != null ? item.CreationDate.ToString("dd.MM.yyyy") : string.Empty,
                    Status = item.Status != null ? item.Status.ToString() : string.Empty,
                    EndDate = item.ConsiderationDate != null ? item.ConsiderationDate.ToString("dd.MM.yyyy") : string.Empty,
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

        public async Task<CooperationModel> CreateCooperation(CooperationModel newCooperation, int requestId, int topicId, string studentId)
        {
            newCooperation.Teacher = await GetCurrentTeacherUser();
            newCooperation.Topic = await _unitOfWork.Topics.SingleOrDefaultAsync(t => t.Id == topicId);
            newCooperation.Student = await _unitOfWork.Students.SingleOrDefaultAsync(s => s.Id.Equals(studentId));
            newCooperation.StartDate = System.DateTime.Now;
            newCooperation.Status = CooperationStatus.Active;

            await _unitOfWork.Cooperations.AddAsync(newCooperation);
            await _cooperationRequestService.AcceptCooperationRequest(requestId);
            await _unitOfWork.CommitAsync();

            return newCooperation;
        }

        public async Task UpdateCooperation(int id, CooperationModel updatedCooperation)
        {
            var cooperationToBeUpdated = await GetCooperationForCurrentUserById(id);

            // TODO updates on fields that can be updated

            await _unitOfWork.CommitAsync();
        }

        public async Task EndCooperation(int id)
        {
            var cooperationToBeEnded = await GetCooperationForCurrentUserById(id);
            if (!CanBeEnded(cooperationToBeEnded))
            {
                throw new Exception("Cooperation cannot be deleted!");
            }

            cooperationToBeEnded.Status = CooperationStatus.Ended;
            await _unitOfWork.CommitAsync();
        }

        public async Task ArchiveCooperation(int id)
        {
            var cooperationToBeArchived = await GetCooperationForCurrentUserById(id);
            if (!CanBeArchived(cooperationToBeArchived))
            {
                throw new Exception("Cooperation cannot be archived!");
            }

            cooperationToBeArchived.Status = CooperationStatus.Archived;
            await _unitOfWork.CommitAsync();
        }

        private async Task<Boolean> IsCurrentUserAllowedToModify(CooperationModel Cooperation)
        {
            var currentUserId = await _userManager.GetCurrentUserId();
            return (currentUserId.Equals(Cooperation.Student!.Id) || currentUserId.Equals(Cooperation.Teacher!.Id));
        }

        private async Task<StudentUser> GetCurrentStudentUser()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var currentUser = await _userManager.GetUserAsync(httpContext!.User);
            return await _unitOfWork.Students.SingleOrDefaultAsync(t => t.Id == currentUser.Id);
        }

        private async Task<TeacherUser> GetCurrentTeacherUser()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var currentUser = await _userManager.GetUserAsync(httpContext!.User);
            return await _unitOfWork.Teachers.SingleOrDefaultAsync(t => t.Id == currentUser.Id);
        }

        private Boolean CanBeEnded(CooperationModel cooperation)
        {
            return cooperation.Status == CooperationStatus.Active;
        }

        private Boolean CanBeArchived(CooperationModel cooperation)
        {
            return cooperation.Status == CooperationStatus.Ended;
        }
    }
}