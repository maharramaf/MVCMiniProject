using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Platforms;
using MVCMiniProject.ViewModels.Visions;

namespace MVCMiniProject.Services
{
    public class VisionAboutService : IVisionAboutService
    {

        private readonly AppDbContext _context;
        public VisionAboutService(AppDbContext context)
        {
            _context = context;
        }
        public async  Task<VisionUIVM> GetAllUIAsync()
        {
            var visions = await _context.VisionAbouts.OrderByDescending(m => m.Id).Select(m => new VisionUIVM
            {
                Description = m.Description,
                Title = m.Title,
                Image = m.Image
            }).FirstOrDefaultAsync();
            return visions;
        }
    }
}
