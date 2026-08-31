using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Courses;

namespace MVCMiniProject.Services
{
    public class CourseService : ICourseService
    {

        private readonly AppDbContext _context;
        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseInfoUIVM>> GetAllUIAsync()
        {
            var courses = await _context.CourseInfos.Include(c => c.CourseImages).Include(a => a.Teacher).Select(c => new CourseInfoUIVM
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                SalesCount = c.SalesCount,
                IsFeature = c.IsFeature,
                IsNew = c.IsNew,
                TeacherName = c.Teacher.FullName,
                MainImage = c.CourseImages.FirstOrDefault(c => c.IsMain).Name,
                TeacherImage = c.Teacher.Image
            }).ToListAsync();
            return courses;
        }
    }
}
