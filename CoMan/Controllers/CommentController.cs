using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using CoMan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // POST: /LoadCommentsTable
        [Authorize(Policy = "CommentCooperations")]
        [HttpPost("LoadCommentsTable")]
        public async Task<IActionResult> LoadTopicTable([FromBody] DtParameters dtParameters)
        {
            try
            {
                var data = await _commentService.GetCommentsForTopic(dtParameters);

                return Json(
                    new DtResult<CommentDatatable>
                    {
                        Draw = dtParameters.Draw,
                        Data = data
                    });
            }
            catch (Exception ex)
            {
                return Json(
                    new DtResult<CommentDatatable>
                    {
                        Error = ex.Message,
                    });
            }
        }

        // POST: /AddComment
        [Authorize(Policy = "CommentCooperations")]
        [HttpPost("AddComment")]
        public async Task<ActionResult> CreateAsync(string comment, int cooperationId)
        {
            try
            {
                await _commentService.CreateComment(comment, cooperationId);
                return Json(
                    new
                    {
                        Success = true
                    });
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        Success = false,
                        Errors = ex.Message
                    });
            }
        }
    }
}
