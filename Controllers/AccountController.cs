using Microsoft.AspNetCore.Mvc;

namespace GOTG_Ronewa.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
