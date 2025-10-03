using System;
using System.Threading.Tasks;
using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GOTG.Ronewa.Web.Controllers
{
    [Authorize]
    public class IncidentReportsController : Controller
    {
        private readonly IIncidentService _incidentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IncidentReportsController(IIncidentService incidentService, UserManager<ApplicationUser> userManager)
        {
            _incidentService = incidentService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _incidentService.GetAllAsync();
            return View(list);
        }

        // GET: /IncidentReports/Create
        public IActionResult Create()
        {
            // Initialize required properties to satisfy the compiler
            var model = new IncidentReport
            {
                Title = string.Empty,
                Location = string.Empty,
                Description = string.Empty,
                Phone = string.Empty
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidentReport model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            model.ReportedByUserId = user?.Id;
            model.ReportedAt = DateTime.UtcNow;
            model.Status = IncidentStatus.Reported;

            await _incidentService.CreateAsync(model);

            TempData["Success"] = "Incident reported. Thank you for your vital contribution.";
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _incidentService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
    }
}