using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.ViewComponents
{
    public class VideoViewComponent : ViewComponent
    {
        private readonly IVideoService _videoService;
        public VideoViewComponent(IVideoService videoService)
        {
            _videoService = videoService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var videos = await _videoService.GetAllUIAsync();
            return View(videos);
        }
    }
}
