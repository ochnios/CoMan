using CoMan.Models;
using CoMan.Models.AuxiliaryModels;

namespace CoMan.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDatatable>> GetCommentsForTopic(DtParameters dtParameters);
        Task<CommentModel> CreateComment(string comment, int cooperationId);
    }
}