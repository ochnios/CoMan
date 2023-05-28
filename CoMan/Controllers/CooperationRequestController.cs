using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
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
        public ActionResult Index()
        {
            return View();
        }

        // POST: /LoadCooperationRequestTable
        [HttpPost("LoadCooperationRequestTable")]
        public async Task<IActionResult> LoadCooperationRequestTable([FromBody] DtParameters dtParameters)
        {
            var data = await _cooperationRequestService.FindDatablesForCurrentUser(dtParameters);

            return Json(
                new DtResult<CooperationRequestDatatable>
                {
                    Draw = dtParameters.Draw,
                    RecordsTotal = data.TotalCount,
                    RecordsFiltered = data.FilteredCount,
                    Data = data.ResultsForTable
                });
        }

        // GET: CooperationRequest/Details/{id}
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

        // GET: CooperationRequest/Edit/{id}
        public async Task<ActionResult> EditAsync(int id)
        {
            return View(await _cooperationRequestService.GetCooperationRequestById(id));
        }

        // POST: CooperationRequest/Edit/{id}
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

        // GET: CooperationRequest/Delete/{id}
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return View(await _cooperationRequestService.GetCooperationRequestById(id));
        }

        // POST: CooperationRequest/Delete/{id}
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
