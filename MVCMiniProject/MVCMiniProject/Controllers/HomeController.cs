using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels;

namespace MVCMiniProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIconService _iconService;
        private readonly ISliderService _sliderService;
        private readonly ISettingService _settingService;
        private readonly IEventService _eventService;

        public HomeController(IIconService iconService,
                              ISliderService sliderService,
                              ISettingService settingService,
                              IEventService eventService)
        {
            _iconService = iconService;
            _sliderService = sliderService;
            _settingService = settingService;
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var icons = await _iconService.GetAllUIAsync();
            var sliders = await _sliderService.GetAllUIAsync();
            var settings = await _settingService.GetAllUIAsync();
            var evenets = await _eventService.GetAllUIAsync();

            return View(new HomeVM
            {
                Icon = icons,
                Slider = sliders,
                Settings = settings,
                Events = evenets

                
            });
        }
    }
}
