using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Icons;
using MVCMiniProject.ViewModels.Sliders;

namespace MVCMiniProject.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;

        public SliderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SliderUIVM>> GetAllUIAsync()
        {
            IEnumerable<SliderUIVM> sliders = await _context.Sliders.Select(m => new SliderUIVM
            {
                Logo = m.Logo,
                Description = m.Description,
                Image = m.Image,
                Name = m.Name
            }).ToListAsync();
            return sliders;
        }
    }
}
