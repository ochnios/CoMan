using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using CoMan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    [Authorize]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        // GET: Topic
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        // POST: /LoadTopicTable
        [AllowAnonymous]
        [HttpPost("LoadTopicTable")]
        public async Task<IActionResult> LoadTopicTable([FromBody] DtParameters dtParameters)
        {
            try
            {
                var data = await _topicService.FindForDatables(dtParameters);

                return Json(
                    new DtResult<TopicDatatable>
                    {
                        Draw = dtParameters.Draw,
                        RecordsTotal = data.TotalCount,
                        RecordsFiltered = data.FilteredCount,
                        Data = data.ResultsForTable
                    });
            }
            catch (Exception ex)
            {
                return Json(
                    new DtResult<TopicDatatable>
                    {
                        Error = ex.Message,
                    });
            }

        }

        // GET: Topic/Details/{id}
        [AllowAnonymous]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            return View(await _topicService.GetTopicById(id));
        }

        // GET: Topic/Create
        [Authorize(Policy = "RequireTeacher")]
        public ActionResult Create()
        {
            return View(new TopicModel());
        }

        // POST: Topic/Create
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(TopicModel topic)
        {
            try
            {
                await _topicService.CreateTopic(topic);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Topic/Edit/{id}
        [Authorize(Policy = "ModifyTopics")]
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                return View(await _topicService.GetTopicForModificationById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Topic/Edit/{id}
        [Authorize(Policy = "ModifyTopics")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, TopicModel updatedTopic)
        {
            try
            {
                TopicModel topicToBeUpdated = await _topicService.GetTopicForModificationById(id);
                await _topicService.UpdateTopic(topicToBeUpdated, updatedTopic);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Topic/Delete/{id}
        [Authorize(Policy = "ModifyTopics")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                return View(await _topicService.GetTopicForModificationById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Topic/Delete/{id}
        [Authorize(Policy = "ModifyTopics")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, TopicModel deletedTopic)
        {
            try
            {
                TopicModel topicToBeDeleted = await _topicService.GetTopicForModificationById(id);
                await _topicService.DeleteTopic(topicToBeDeleted);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
