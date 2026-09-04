using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;

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
    }
}
