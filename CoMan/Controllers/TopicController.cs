using System;
using CoMan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    public class TopicController : Controller
    {
        private readonly IRepository<TopicModel> _topicRepository;

        public TopicController(IRepository<TopicModel> topicRepository)
        {
            _topicRepository = topicRepository;
        }

        // GET: Topic
        public ActionResult Index()
        {
            return View(_topicRepository.List());
        }

        // GET: Topic/Details/5
        public ActionResult Details(int id)
        {
            return View(_topicRepository.GetById(id));
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
                topicModel.AddedDate = System.DateTime.Now;
                _topicRepository.Insert(topicModel);
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
            return View(_topicRepository.GetById(id));
        }

        // POST: TopicController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TopicModel topicModel)
        {
            try
            {
                _topicRepository.Update(topicModel);
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
            return View(_topicRepository.GetById(id));
        }

        // POST: Topic/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TopicModel topicModel)
        {
            try
            {
                _topicRepository.Delete(topicModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
