using CoMan.Models;
using CoMan.Models.AuxiliaryModels;
using CoMan.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoMan.Controllers
{
    [Authorize]
    public class CooperationController : Controller
    {
        private readonly ICooperationService _cooperationService;

        public CooperationController(ICooperationService cooperationService)
        {
            _cooperationService = cooperationService;
        }

        // GET: Cooperation
        public ActionResult Index()
        {
            return View();
        }

        // POST: /LoadCooperationTable
        [Authorize(Policy = "ViewCooperations")]
        [HttpPost("LoadCooperationTable")]
        public async Task<IActionResult> LoadCooperationTable([FromBody] DtParameters dtParameters)
        {
            try
            {
                var data = await _cooperationService.FindDatablesForCurrentUser(dtParameters);

                return Json(
                    new DtResult<CooperationDatatable>
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
                    new DtResult<CooperationDatatable>
                    {
                        Error = ex.Message,
                    });
            }
        }

        // GET: Cooperation/Details/{id}
        [Authorize(Policy = "ViewCooperations")]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                return View(await _cooperationService.GetCooperationForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Policy = "RequireTeacher")]
        // GET: Cooperation/Create
        public ActionResult Create(int requestId, int topicId, string studentId, string topicTitle, string studentEmail, string studentComment)
        {
            ViewBag.RequestId = requestId;
            ViewBag.TopicId = topicId;
            ViewBag.StudentId = studentId;
            ViewBag.TopicTitle = topicTitle;
            ViewBag.StudentEmail = studentEmail;
            ViewBag.StudentComment = studentComment;
            return View(new CooperationModel());
        }

        // POST: Cooperation/Create
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CooperationModel cooperation, int requestId, int topicId, string studentId)
        {
            try
            {
                await _cooperationService.CreateCooperation(cooperation, requestId, topicId, studentId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Cooperation/Edit/{id}
        [Authorize(Policy = "RequireTeacher")]
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                return View(await _cooperationService.GetCooperationForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Cooperation/Edit/{id}
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, CooperationModel updatedCooperation)
        {
            try
            {
                await _cooperationService.UpdateCooperation(id, updatedCooperation);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        // GET: Cooperation/Accept/{id}
        [Authorize(Policy = "RequireTeacher")]
        public async Task<ActionResult> EndAsync(int id)
        {
            try
            {
                CooperationModel cooperation = await _cooperationService.GetCooperationForCurrentUserById(id);
                ViewBag.TopicTitle = cooperation.Topic!.Title;
                ViewBag.StudentEmail = cooperation.Student!.Email;
                return View(await _cooperationService.GetCooperationForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Cooperation/Accept/{id}
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EndAsync(int id, CooperationModel endedCooperation)
        {
            try
            {
                await _cooperationService.EndCooperation(id, endedCooperation);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Cooperation/Archive/{id}
        [Authorize(Policy = "RequireTeacher")]
        public async Task<ActionResult> ArchiveAsync(int id)
        {
            try
            {
                return View(await _cooperationService.GetCooperationForCurrentUserById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Cooperation/Archive/{id}
        [Authorize(Policy = "RequireTeacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ArchiveAsync(int id, CooperationModel archivedCooperation)
        {
            try
            {
                await _cooperationService.ArchiveCooperation(id);
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
