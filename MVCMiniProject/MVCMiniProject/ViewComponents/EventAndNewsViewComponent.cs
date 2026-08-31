using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.CoursesAndSetting;
using MVCMiniProject.ViewModels.EventAndNews;
using System.Net.WebSockets;

namespace MVCMiniProject.ViewComponents
{
    public class EventAndNewsViewComponent : ViewComponent
    {
        private readonly IEventService _eventService;
        private readonly INewsService _newsService;
        public EventAndNewsViewComponent(IEventService eventService,
                                          INewsService newsService)
        {
            _eventService = eventService;
            _newsService = newsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _eventService.GetAllUIAsync();
            var news = await _newsService.GetAllUIAsync();
            return View(new EventAndNewsVM
            {
               Events = events ,
               News = news
            });
        }
    }
}
