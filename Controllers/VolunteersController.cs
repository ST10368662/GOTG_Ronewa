using GOTG.Ronewa.Web.Models;
using GOTG.Ronewa.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace GOTG.Ronewa.Web.Controllers
{
    [Authorize]
    public class VolunteersController : Controller
    {
        private readonly IVolunteerService _volunteerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VolunteersController(IVolunteerService volunteerService, UserManager<ApplicationUser> userManager)
        {
            _volunteerService = volunteerService;
            _userManager = userManager;
        }

        // GET: /Volunteers (Displays a list of all registered volunteers)
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var volunteers = await _volunteerService.GetAllVolunteersAsync();
            return View(volunteers);
        }

        // GET: /Volunteers/Register
        public IActionResult Register()
        {
            var model = new VolunteerProfile { Phone = string.Empty };
            return View(model);
        }

        // POST: /Volunteers/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(VolunteerProfile model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Assign the UserId from the logged-in user
            model.UserId = user.Id;

            // This line removes the validation error for UserId
            // so ModelState.IsValid becomes true
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
            {
                // The form still has other validation errors (e.g., empty date of birth)
                return View(model);
            }

            var existingProfile = await _volunteerService.GetVolunteerProfileByUserIdAsync(model.UserId);
            if (existingProfile != null)
            {
                TempData["Info"] = "You have already registered as a volunteer.";
                return RedirectToAction(nameof(Index));
            }

            await _volunteerService.CreateVolunteerProfileAsync(model);

            TempData["Success"] = "Thank you for registering! Your information has been recorded.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Volunteers/MyTasks (Displays tasks assigned to the current user)
        public async Task<IActionResult> MyTasks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var list = await _volunteerService.GetAssignedTasksAsync(user.Id);
            return View(list);
        }

        // GET: /Volunteers/BrowseTasks (Displays all tasks for volunteers to browse)
        public async Task<IActionResult> BrowseTasks()
        {
            var tasks = await _volunteerService.GetAllTasksAsync();
            return View(tasks);
        }

        // POST: /Volunteers/AcceptTask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptTask(int taskId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            await _volunteerService.AssignTaskAsync(taskId, user.Id);

            TempData["Success"] = "Task assigned successfully!";
            return RedirectToAction(nameof(MyTasks));
        }

        // GET: /Volunteers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var volunteer = await _volunteerService.GetVolunteerProfileByIdAsync(id);

            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }
    }
}

