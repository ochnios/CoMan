using CoMan.Models;
using CoMan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    [Authorize]
    public class CooperationRequestController : Controller
    {
        private readonly ICooperationRequestService _cooperationRequestService;

        public CooperationRequestController(ICooperationRequestService cooperationRequestService)
        {
            _cooperationRequestService = cooperationRequestService;
        }

        // GET: CooperationRequest
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _cooperationRequestService.GetAllCooperationRequests());
        }

        // GET: CooperationRequest/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            return View(await _cooperationRequestService.GetCooperationRequestById(id));
        }

        [Authorize(Policy = "RequireStudent")]
        // GET: CooperationRequest/Create
        public ActionResult Create(int topicId, string teacherId)
        {
            ViewBag.TopicId = topicId;
            ViewBag.TeacherId = teacherId;
            return View(new CooperationRequestModel());
        }

        // POST: CooperationRequest/Create
        [Authorize(Policy = "RequireStudent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CooperationRequestModel cooperationRequest, int topicId, string teacherId)
        {
            try
            {
                await _cooperationRequestService.CreateCooperationRequest(cooperationRequest, topicId, teacherId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CooperationRequest/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _cooperationRequestService.GetCooperationRequestById(id));
        }

        // POST: CooperationRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, CooperationRequestModel cooperationRequest)
        {
            try
            {
                CooperationRequestModel cooperationRequestToBeUpdated = await _cooperationRequestService.GetCooperationRequestById(id);
                await _cooperationRequestService.UpdateCooperationRequest(cooperationRequestToBeUpdated, cooperationRequest);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CooperationRequest/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _cooperationRequestService.GetCooperationRequestById(id));
        }

        // POST: CooperationRequest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, CooperationRequestModel cooperationRequest)
        {
            try
            {
                await _cooperationRequestService.DeleteCooperationRequest(cooperationRequest);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
