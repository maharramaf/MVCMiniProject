using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetAllUIAsync()
        {
            var settings = await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
            return settings;
        }

        public async Task<IEnumerable<Setting>> GetAllAsync()
        {
            return await _context.Settings.ToListAsync();
        }

        public async Task<Setting> GetByIdAsync(int id)
        {
            return await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(SettingCreateVM settingVM)
        {
            Setting setting = new Setting
            {
                Key = settingVM.Key,
                Value = settingVM.Value
            };

            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SettingUpdateVM settingVM)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == settingVM.Id);
            if (setting == null) return;

            setting.Key = settingVM.Key;
            setting.Value = settingVM.Value;

            _context.Settings.Update(setting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return;

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
        }
    }
}
