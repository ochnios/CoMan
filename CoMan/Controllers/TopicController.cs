using CoMan.Models;
using CoMan.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        // GET: Topic
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _topicService.GetAllTopics());
        }

        // GET: Topic/Details/{id}
        public async Task<ActionResult> DetailsAsync(int id)
        {
            return View(await _topicService.GetTopicById(id));
        }

        // GET: Topic/Create
        public ActionResult Create()
        {
            return View(new TopicModel());
        }

        // POST: Topic/Create
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
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _topicService.GetTopicById(id));
        }

        // POST: TopicController/Edit/{id}
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
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _topicService.GetTopicById(id));
        }

        // POST: Topic/Delete/{id}
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
