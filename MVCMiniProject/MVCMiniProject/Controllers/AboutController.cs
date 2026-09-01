using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels;
using MVCMiniProject.ViewModels.Platforms;
using MVCMiniProject.ViewModels.Teachers;
using MVCMiniProject.ViewModels.Visions;

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
