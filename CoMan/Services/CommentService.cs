using CoMan.Models;
using CoMan.Data;
using Microsoft.AspNetCore.Identity;
using CoMan.Models.AuxiliaryModels;
using CoMan.Extensions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace CoMan.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CommentDatatable>> GetCommentsForTopic(DtParameters dtParameters)
        {
            var orderAscendingDirection = true;
            if (dtParameters.Order != null)
            {
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            int? cooperationId = -1;
            if (dtParameters.CooperationId != null)
            {
                cooperationId = dtParameters.CooperationId;
            }

            var cooperation = await _unitOfWork.Cooperations.SingleOrDefaultAsync(c => c.Id == cooperationId);
            var rawResults = await _unitOfWork.Comments
                .GetFilteredAsync(c =>
                    c.Cooperation!.Id == cooperationId &&
                    (
                        c.Author!.Id.Equals(cooperation.Teacher!.Id) ||
                        c.Author!.Id.Equals(cooperation.Student!.Id)
                    )
                );

            if (orderAscendingDirection)
            {
                rawResults = rawResults.OrderBy(c => c.AddedDate);
            }
            else
            {
                rawResults = rawResults.OrderByDescending(c => c.AddedDate);
            }

            List<CommentDatatable> resultsForDatatable = new();
            foreach (var item in rawResults)
            {
                resultsForDatatable.Add(new CommentDatatable()
                {
                    Id = item.Id,
                    AddedDate = item.AddedDate.ToString("dd.MM.yyyy HH:mm"),
                    Comment = item.Comment,
                    Author = item.Author != null ? item.Author.FirstName + " " + item.Author.LastName : string.Empty,
                });
            }

            return resultsForDatatable;
        }

        public async Task<CommentModel> CreateComment(string comment, int cooperationId)
        {
            var newComment = new CommentModel();
            var cooperation = await _unitOfWork.Cooperations.SingleOrDefaultAsync(c => c.Id == cooperationId);
            var author = await _userManager.GetCurrentApplicationUser();

            if (!IsUserAllowedToAddComment(cooperation, author))
            {
                throw new Exception("You are not allowed to add comment for this cooperation!");
            }

            newComment.AddedDate = System.DateTime.Now;
            newComment.Comment = comment;
            newComment.Cooperation = cooperation;
            newComment.Author = author;

            await _unitOfWork.Comments.AddAsync(newComment);
            await _unitOfWork.CommitAsync();

            return newComment;
        }

        private Boolean IsUserAllowedToAddComment(CooperationModel cooperation, ApplicationUser user)
        {
            return cooperation.Student.Id == user.Id || cooperation.Teacher.Id == user.Id
                && cooperation.Status == CooperationStatus.Active;
        }
    }
}