using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels;
using MVCMiniProject.ViewModels.Platforms;
using MVCMiniProject.ViewModels.Visions;

namespace MVCMiniProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly IPlatformAboutService _platformAboutService;
        private readonly IVisionAboutService _visionAboutService;
        public AboutController(IPlatformAboutService platformAboutService,
                                IVisionAboutService visionAboutService)
        {
            _platformAboutService = platformAboutService;
             _visionAboutService = visionAboutService;
        }
        public async Task <IActionResult> Index()
        {
            PlatformUIVM platform = await _platformAboutService.GetAllUIAsync();
            VisionUIVM vision = await _visionAboutService.GetAllUIAsync();
             return View(new AboutVM
             {
                 Platform = platform,
                 Vision = vision
             });
        }
    }
}
