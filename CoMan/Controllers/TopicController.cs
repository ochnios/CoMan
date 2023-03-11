using System;
using CoMan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    public class TopicController : Controller
    {
        private static readonly IList<TopicModel> _topics = new List<TopicModel>()
        {
            new TopicModel() { TopicId = 1, AddedDate = System.DateTime.Now, Title = "title1",
                Description = "des1", StudentLimit = 1, Status = TopicStatus.Active },
            new TopicModel() { TopicId = 2, AddedDate = System.DateTime.Now, Title = "title2",
                Description = "des2", StudentLimit = 2, Status = TopicStatus.Active }
        };

        // GET: Topic
        public ActionResult Index()
        {
            return View(_topics);
        }

        // GET: Topic/Details/5
        public ActionResult Details(int id)
        {
            return View(_topics.FirstOrDefault(x => x.TopicId == id));
        }

        // GET: Topic/Create
        public ActionResult Create()
        {
            return View(new TopicModel());
        }

        // POST: Topic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TopicModel topicModel)
        {
            try
            {
                topicModel.TopicId = _topics.Count;
                _topics.Add(topicModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Topic/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_topics.FirstOrDefault(x => x.TopicId == id));
        }

        // POST: TopicController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TopicModel topicModel)
        {
            try
            {
                TopicModel topic = _topics.FirstOrDefault(y => y.TopicId == id);
                topic.Title = topicModel.Title;
                topic.Description = topicModel.Description;
                topic.Status = topicModel.Status;
                topic.StudentLimit = topicModel.StudentLimit;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Topic/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_topics.FirstOrDefault(x => x.TopicId == id));
        }

        // POST: Topic/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TopicModel topicModel)
        {
            try
            {
                TopicModel topic = _topics.FirstOrDefault(x => x.TopicId == id);
                _topics.Remove(topic);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
