using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.Services
{
    public class PositionService : IPositionService
    {
        private readonly AppDbContext _context;

        public PositionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<Position> GetByIdAsync(int id)
        {
            return await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
