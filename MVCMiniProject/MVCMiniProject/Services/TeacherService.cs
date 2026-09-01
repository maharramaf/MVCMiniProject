using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Teachers;

namespace MVCMiniProject.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;
        public TeacherService (AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TeacherUIVM>> GetAllUIAsync()
        {
            var teachers = await _context.Teachers.Include(m => m.Position).Select(m => new TeacherUIVM
            {
                FullName = m.FullName,
                Image = m.Image,
                Position = m.Position.Name
            }).ToListAsync();
            return teachers;
        }
    }
}
