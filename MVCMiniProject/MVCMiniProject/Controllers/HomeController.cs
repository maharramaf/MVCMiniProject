using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels;

namespace MVCMiniProject.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {


            return View();
        }
    }
}
