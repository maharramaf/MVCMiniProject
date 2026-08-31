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
        private readonly INewsService _newsService;
        private readonly IVideoService _videoService;
        private readonly ICourseService _courseService;

        public HomeController(IIconService iconService,
                              ISliderService sliderService,
                              ISettingService settingService,
                              IEventService eventService,
                              INewsService newsService,
                              IVideoService videoService,
                              ICourseService courseService)
        {
            _iconService = iconService;
            _sliderService = sliderService;
            _settingService = settingService;
            _eventService = eventService;
            _newsService = newsService;
            _videoService = videoService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var icons = await _iconService.GetAllUIAsync();
            var sliders = await _sliderService.GetAllUIAsync();
            var settings = await _settingService.GetAllUIAsync();
            var evenets = await _eventService.GetAllUIAsync();
            var news = await _newsService.GetAllUIAsync();
            var videos = await _videoService.GetAllUIAsync();
            var courses = await _courseService.GetAllUIAsync();

            return View(new HomeVM
            {
                Icon = icons,
                Slider = sliders,
                Settings = settings,
                Events = evenets,
                News = news,
                Video = videos,
                CourseInfos = courses
                
             });
        }
    }
}
