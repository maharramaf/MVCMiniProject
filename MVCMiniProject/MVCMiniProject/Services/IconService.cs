using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;
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

        public async Task<IEnumerable<Icon>> GetAllAsync()
        {
            return await _context.Icons.ToListAsync();
        }

        public async Task<Icon> GetByIdAsync(int id)
        {
            return await _context.Icons.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task CreateAsync(IconCreateVM iconVM)
        {
            Icon icon = new Icon
            {
                Name = iconVM.Name
            };

            await _context.Icons.AddAsync(icon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IconUpdateVM iconVM)
        {
            var icon = await _context.Icons.FirstOrDefaultAsync(i => i.Id == iconVM.Id);
            if (icon == null) return;

            icon.Name = iconVM.Name;

            _context.Icons.Update(icon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var icon = await _context.Icons.FirstOrDefaultAsync(i => i.Id == id);
            if (icon == null) return;

            _context.Icons.Remove(icon);
            await _context.SaveChangesAsync();
        }
    }
}
