using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GOTG.Ronewa.Web.Controllers
{
    [Authorize]
    public class DonationsController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationsController(IDonationService donationService, UserManager<ApplicationUser> userManager)
        {
            _donationService = donationService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _donationService.GetAllAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View(new Donation());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donation model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            model.DonorId = user?.Id;
            model.DonatedAt = DateTime.UtcNow;

            await _donationService.CreateAsync(model);

            // Redirect to the Index page to show the list of donations
            TempData["Success"] = "Thanks — your donation was recorded.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _donationService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }
    }
}