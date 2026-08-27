using Microsoft.AspNetCore.Mvc;

namespace MVCMiniProject.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
