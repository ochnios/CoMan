using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using System.Collections;

namespace CoMan.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDatatable>> GetCommentsForTopic(DtParameters dtParameters);
        Task<CommentModel> CreateComment(string comment, int cooperationId);
    }
}