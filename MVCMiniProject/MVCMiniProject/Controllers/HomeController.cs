using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.ViewModels;

namespace MVCMiniProject.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View(new HomeVM());
        }
    }
}
