using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Platforms;
using MVCMiniProject.ViewModels.Videos;

namespace MVCMiniProject.Services
{
    public class PlatformAboutService : IPlatformAboutService
    {
        private readonly AppDbContext _context;
        public PlatformAboutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PlatformUIVM> GetAllUIAsync()
        {
            var platforms = await _context.PlatformAbouts.OrderByDescending(m => m.Id).Select(m => new PlatformUIVM
            {
                Description = m.Description,
                Title = m.Title,
                Image = m.Image
            }).FirstOrDefaultAsync();
            return platforms;
        }
    }
}
