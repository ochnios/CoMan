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
        [Authorize(Policy = "ViewCooperationRequests")]
        [HttpPost("LoadCooperationRequestTable")]
        public async Task<IActionResult> LoadCooperationRequestTable([FromBody] DtParameters dtParameters)
        {
            try
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
            catch (Exception ex)
            {
                return Json(
                    new DtResult<TopicDatatable>
                    {
                        Error = ex.Message,
                    });
            }
        }

        // GET: CooperationRequest/Details/{id}
        [Authorize(Policy = "ViewCooperationRequests")]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                return View(await _cooperationRequestService.GetCooperationRequestForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: CooperationRequest/Edit/{id}
        [Authorize(Policy = "RequireStudent")]
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                return View(await _cooperationRequestService.GetCooperationRequestForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: CooperationRequest/Edit/{id}
        [Authorize(Policy = "RequireStudent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, CooperationRequestModel updatedCooperationRequest)
        {
            try
            {
                await _cooperationRequestService.UpdateCooperationRequest(id, updatedCooperationRequest);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        // GET: CooperationRequest/Accept/{id}
        [Authorize(Policy = "RequireTeacher")]
        public async Task<ActionResult> AcceptAsync(int id)
        {
            try
            {
                return View(await _cooperationRequestService.GetCooperationRequestForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: CooperationRequest/Accept/{id}
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AcceptAsync(int id, CooperationRequestModel acceptedCooperationRequest)
        {
            try
            {
                await _cooperationRequestService.AcceptCooperationRequest(id, acceptedCooperationRequest);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: CooperationRequest/Reject/{id}
        [Authorize(Policy = "RequireTeacher")]
        public async Task<ActionResult> RejectAsync(int id)
        {
            try
            {
                return View(await _cooperationRequestService.GetCooperationRequestForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: CooperationRequest/Reject/{id}
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectAsync(int id, CooperationRequestModel rejectedCooperationRequest)
        {
            try
            {
                await _cooperationRequestService.RejectCooperationRequest(id, rejectedCooperationRequest);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: CooperationRequest/Archive/{id}
        [Authorize(Policy = "RequireTeacher")]
        public async Task<ActionResult> ArchiveAsync(int id)
        {
            try
            {
                return View(await _cooperationRequestService.GetCooperationRequestForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: CooperationRequest/Archive/{id}
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ArchiveAsync(int id, CooperationRequestModel archivedCooperationRequest)
        {
            try
            {
                await _cooperationRequestService.ArchiveCooperationRequest(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: CooperationRequest/Delete/{id}
        [Authorize(Policy = "RequireStudent")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                return View(await _cooperationRequestService.GetCooperationRequestForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: CooperationRequest/Delete/{id}
        [Authorize(Policy = "RequireStudent")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, CooperationRequestModel deletedCooperationRequest)
        {
            try
            {
                await _cooperationRequestService.DeleteCooperationRequest(id);
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
