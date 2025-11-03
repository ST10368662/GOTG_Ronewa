using Microsoft.AspNetCore.Mvc;

namespace GOTG.Ronewa.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About() => View();
    }
}
