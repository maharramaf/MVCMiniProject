using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.PlatformAndVision;

namespace MVCMiniProject.ViewComponents
{
    public class PlatformAndVisionViewComponent : ViewComponent
    {
        private readonly IPlatformAboutService _platfromAboutService;
        private readonly IVisionAboutService _visionAboutService;
        public PlatformAndVisionViewComponent(IPlatformAboutService platfromAboutService,
                                              IVisionAboutService visionAboutService)
        {
            _platfromAboutService = platfromAboutService;
            _visionAboutService = visionAboutService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var platfrom = await _platfromAboutService.GetAllUIAsync();
            var vision = await _visionAboutService.GetAllUIAsync();
            return View(new PlatformAndVisionVM
            {
                Platform = platfrom,
                Vision = vision
            });
        }
    }
}
