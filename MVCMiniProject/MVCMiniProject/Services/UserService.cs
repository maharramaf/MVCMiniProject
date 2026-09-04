using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByUsernameAsync(string username)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.AppUsers.AnyAsync(u => u.Email == email);
        }

        public async Task<AppUser> CreateAsync(AppUser user)
        {
            await _context.AppUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(AppUser user)
        {
            _context.AppUsers.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<AppUser?> GetByVerificationHashAsync(string hash)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.VerificationCode == hash);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.AppUsers.CountAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _context.AppUsers
                .OrderBy(u => u.Username)
                .ToListAsync();
        }
    }
}
