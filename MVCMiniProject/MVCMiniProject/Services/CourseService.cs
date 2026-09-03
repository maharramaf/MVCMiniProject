using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Courses;

namespace MVCMiniProject.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        public async Task<CourseDetailVM> GetByIdAsync(int id)
        {
            var course = await _context.CourseInfos
                .Include(c => c.CourseImages)
                .Include(a => a.Teacher)
                .Where(c => c.Id == id)
                .Select(c => new CourseDetailVM
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    SalesCount = c.SalesCount,
                    IsFeature = c.IsFeature,
                    IsNew = c.IsNew,
                    TeacherName = c.Teacher.FullName,
                    MainImage = c.CourseImages.FirstOrDefault(img => img.IsMain).Name,
                    TeacherImage = c.Teacher.Image,
                    CourseImages = c.CourseImages.Where(img => !img.IsMain).Select(img => img.Name).ToList()
                })
                .FirstOrDefaultAsync();

            return course;
        }

        public async Task<IEnumerable<CourseInfo>> GetAllAsync()
        {
            return await _context.CourseInfos.Include(c => c.CourseImages).Include(c => c.Teacher).ToListAsync();
        }

        public async Task<CourseInfo> GetCourseByIdAsync(int id)
        {
            return await _context.CourseInfos.Include(c => c.CourseImages).Include(c => c.Teacher).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(CourseCreateVM courseVM)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(courseVM.MainImage.FileName);
            string path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await courseVM.MainImage.CopyToAsync(stream);
            }

            var firstTeacher = await _context.Teachers.FirstOrDefaultAsync();

            CourseInfo course = new CourseInfo
            {
                Title = courseVM.Title,
                Description = courseVM.Description,
                Price = (int)courseVM.Price,
                SalesCount = courseVM.SalesCount,
                IsFeature = courseVM.IsFeature,
                IsNew = courseVM.IsNew,
                TeacherId = firstTeacher?.Id ?? 1,
                CourseImages = new List<CourseImage>
                {
                    new CourseImage { Name = fileName, IsMain = true }
                }
            };

            await _context.CourseInfos.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseUpdateVM courseVM)
        {
            var course = await _context.CourseInfos.Include(c => c.CourseImages).FirstOrDefaultAsync(c => c.Id == courseVM.Id);
            if (course == null) return;

            if (courseVM.MainImage != null)
            {
                var mainImage = course.CourseImages.FirstOrDefault(img => img.IsMain);
                if (mainImage != null)
                {
                    string oldImagePath = Path.Combine(_env.WebRootPath, "images", mainImage.Name);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(courseVM.MainImage.FileName);
                    string path = Path.Combine(_env.WebRootPath, "images", fileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await courseVM.MainImage.CopyToAsync(stream);
                    }

                    mainImage.Name = fileName;
                }
            }

            course.Title = courseVM.Title;
            course.Description = courseVM.Description;
            course.Price = (int)courseVM.Price;
            course.SalesCount = courseVM.SalesCount;
            course.IsFeature = courseVM.IsFeature;
            course.IsNew = courseVM.IsNew;

            _context.CourseInfos.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.CourseInfos.Include(c => c.CourseImages).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return;

            foreach (var image in course.CourseImages)
            {
                string imagePath = Path.Combine(_env.WebRootPath, "images", image.Name);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.CourseInfos.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
