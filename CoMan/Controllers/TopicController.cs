using CoMan.Data;
using CoMan.Models;
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
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _topicService.GetTopicById(id));
        }

        // POST: TopicController/Edit/{id}
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, TopicModel topic)
        {
            try
            {
                TopicModel topicToBeUpdated = await _topicService.GetTopicById(id);
                await _topicService.UpdateTopic(topicToBeUpdated, topic);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Topic/Delete/{id}
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _topicService.GetTopicById(id));
        }

        // POST: Topic/Delete/{id}
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, TopicModel topic)
        {
            try
            {
                await _topicService.DeleteTopic(topic);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
