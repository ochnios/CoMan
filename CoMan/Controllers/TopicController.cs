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
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _topicService.GetAllTopics());
        }

        // POST: Topic/LoadTable/{id}
        [AllowAnonymous]
        [HttpPost("LoadTable")]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        {
            var data = await _topicService.FindForDatables(dtParameters);

            return Json(
                new DtResult<TopicTable>
                {
                    Draw = dtParameters.Draw,
                    RecordsTotal = data.TotalCount,
                    RecordsFiltered = data.FilteredCount,
                    Data = data.ResultsForTable
                });
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
                return View();
            }
        }

        // GET: Topic/Edit/{id}
        [Authorize(Policy = "ModifyTopics")]
        public async Task<ActionResult> EditAsync(int id)
        {
            TopicModel topicToBeUpdated = await _topicService.GetTopicById(id);
            if (await _topicService.IsUserAllowedToModifyTopic(topicToBeUpdated))
            {
                // TODO Add checking if topic can be modified (there are no current cooperations and coop requests)
                return View(topicToBeUpdated);
            }
            else
            {
                return RedirectToAction("Error", "Home"); // TODO Access Denied
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
                TopicModel topicToBeUpdated = await _topicService.GetTopicById(id);
                if (await _topicService.IsUserAllowedToModifyTopic(topicToBeUpdated))
                {
                    // TODO Add checking if topic can be modified (there are no current cooperations and coop requests)
                    await _topicService.UpdateTopic(topicToBeUpdated, updatedTopic);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Topic/Delete/{id}
        [Authorize(Policy = "ModifyTopics")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            TopicModel topicToBeDeleted = await _topicService.GetTopicById(id);
            if (await _topicService.IsUserAllowedToModifyTopic(topicToBeDeleted))
            {
                // TODO Add checking if topic can be deleted (there are no any cooperations and coop requests connected with topic)
                // (if there are, suggest change status to Archived)
                return View(topicToBeDeleted);
            }
            else
            {
                return RedirectToAction("Error", "Home"); // TODO Access Denied
            }
        }

        // POST: Topic/Delete/{id}
        [Authorize(Policy = "ModifyTopics")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, TopicModel topic)
        {
            try
            {
                if (await _topicService.IsUserAllowedToModifyTopic(topic))
                {
                    // TODO Add checking if topic can be deleted (there are no any cooperations and coop requests connected with topic)
                    // (if there are, suggest change status to Archived)
                    await _topicService.DeleteTopic(topic);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Error", "Home"); // TODO Access Denied
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
