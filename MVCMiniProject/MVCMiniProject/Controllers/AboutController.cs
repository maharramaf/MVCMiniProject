using Microsoft.AspNetCore.Mvc;

namespace MVCMiniProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
