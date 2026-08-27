using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Home;

namespace MVCMiniProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIconService _iconService;
        private readonly ISliderService _sliderService;

        public HomeController(IIconService iconService,
                              ISliderService sliderService)
        {
            _iconService = iconService;
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            var icons = await _iconService.GetAllAsync();
            var sliders = await _sliderService.GetAllAsync();

            return View(new HomeVM
            {
                Icon = icons,
                Slider = sliders
            });
        }
    }
}
