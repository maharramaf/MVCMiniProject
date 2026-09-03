using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Teachers;

namespace MVCMiniProject.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeacherService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.Include(t => t.Position).ToListAsync();
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            return await _context.Teachers.Include(t => t.Position).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateAsync(TeacherCreateVM teacherVM)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(teacherVM.Image.FileName);
            string path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await teacherVM.Image.CopyToAsync(stream);
            }

            Teacher teacher = new Teacher
            {
                FullName = teacherVM.FullName,
                Image = fileName,
                PositionId = teacherVM.PositionId
            };

            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeacherUpdateVM teacherVM)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherVM.Id);
            if (teacher == null) return;

            if (teacherVM.Image != null)
            {
                string oldImagePath = Path.Combine(_env.WebRootPath, "images", teacher.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(teacherVM.Image.FileName);
                string path = Path.Combine(_env.WebRootPath, "images", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await teacherVM.Image.CopyToAsync(stream);
                }

                teacher.Image = fileName;
            }

            teacher.FullName = teacherVM.FullName;
            teacher.PositionId = teacherVM.PositionId;

            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return;

            string imagePath = Path.Combine(_env.WebRootPath, "images", teacher.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
