using CoMan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    public class CooperationRequestController : Controller
    {
        private readonly IRepository<CooperationRequestModel> _cooperationRequestRepository;
        private readonly IRepository<TopicModel> _topicRepository;

        public CooperationRequestController(IRepository<CooperationRequestModel> cooperationRequestRepository)
        {
            _cooperationRequestRepository = cooperationRequestRepository;
        }

        // GET: CooperationRequest
        public ActionResult Index()
        {
            return View(_cooperationRequestRepository.List());
        }

        // GET: CooperationRequest/Details/5
        public ActionResult Details(int id)
        {
            return View(_cooperationRequestRepository.GetById(id));
        }

        // GET: CooperationRequest/Create
        public ActionResult Create()
        {
            return View(new CooperationRequestModel());
        }

        // POST: CooperationRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CooperationRequestModel cooperationRequestModel)
        {
            try
            {
                cooperationRequestModel.CreationDate = System.DateTime.Now;
                cooperationRequestModel.Status = CooperationRequestStatus.Waiting;
                //cooperationRequestModel.Topic = 
                _cooperationRequestRepository.Insert(cooperationRequestModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CooperationRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_cooperationRequestRepository.GetById(id));
        }

        // POST: CooperationRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CooperationRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CooperationRequest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
