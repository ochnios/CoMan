using CoMan.Data;
using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using CoMan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

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

            List<TopicTable> result = new List<TopicTable>();
            foreach (var item in data)
            {
                result.Add(new TopicTable()
                {
                    Id = item.Id,
                    AddedDate = item.AddedDate.ToString("dd.MM.yyyy"),
                    Status = item.Status.ToString(),
                    Title = item.Title,
                    StudentLimit = item.StudentLimit,
                    AuthorId = item.Author.Id,
                    AuthorName = item.Author.FirstName + " " + item.Author.LastName,
                }); ;
            }

            var filteredResultsCount = result.Count();
            var totalResultsCount = await _topicService.CountTopics();

            return Json(
            new DtResult<TopicTable>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = result
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
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _topicService.GetTopicById(id));
        }

        // POST: Topic/Edit/{id}
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
