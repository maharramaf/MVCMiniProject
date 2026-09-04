using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Icons;

namespace MVCMiniProject.Services
{
    public class IconService : IIconService
    {
        private readonly AppDbContext _context;

        public IconService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IconUIVM>> GetAllUIAsync()
        {
            IEnumerable<IconUIVM> brands = await _context.Icons.Select(m => new IconUIVM
            {
                Name = m.Name
            }).ToListAsync();
            return brands;
        }
    }
}
