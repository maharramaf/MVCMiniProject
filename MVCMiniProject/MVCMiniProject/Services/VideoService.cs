using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Videos;

namespace MVCMiniProject.Services
{
    public class VideoService : IVideoService
    {
        private readonly AppDbContext _context;
        public VideoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<VideoUIVM> GetAllUIAsync()
        {
            var videos = await _context.Videos.OrderByDescending(m => m.Id).Select(m => new VideoUIVM
            {
                VideoName = m.VideoName
            }).FirstOrDefaultAsync();
            return videos;
        }
    }
}
