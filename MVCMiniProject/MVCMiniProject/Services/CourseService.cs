using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Helpers;
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

        public async Task<IReadOnlyList<CourseSearchVM>> SearchByTitleAsync(string? query, int take = 8)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Array.Empty<CourseSearchVM>();
            }

            var term = query.Trim().ToLowerInvariant();
            if (term.Length > 80)
            {
                term = term[..80];
            }

            if (take < 1)
            {
                take = 8;
            }

            var pattern = "%" + EscapeLike(term) + "%";

            var rows = await _context.CourseInfos
                .AsNoTracking()
                .Where(c => EF.Functions.Like(c.Title.ToLower(), pattern))
                .OrderBy(c => c.Title)
                .Take(take)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Price,
                    c.Description,
                    Image = c.CourseImages
                        .Where(i => i.IsMain)
                        .Select(i => i.Name)
                        .FirstOrDefault()
                        ?? c.CourseImages.Select(i => i.Name).FirstOrDefault()
                })
                .ToListAsync();

            return rows.Select(c => new CourseSearchVM
            {
                Id = c.Id,
                Title = c.Title,
                Price = c.Price,
                Image = c.Image,
                Excerpt = Truncate(c.Description, 90)
            }).ToList();
        }

        private static string EscapeLike(string value)
        {
            return value
                .Replace("[", "[[]", StringComparison.Ordinal)
                .Replace("%", "[%]", StringComparison.Ordinal)
                .Replace("_", "[_]", StringComparison.Ordinal);
        }

        private static string Truncate(string? text, int max)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var trimmed = text.Trim();
            return trimmed.Length <= max ? trimmed : trimmed[..max].TrimEnd() + "…";
        }

        public async Task<CourseDetailVM?> GetByIdAsync(int id)
        {
            var course = await _context.CourseInfos
                .Include(c => c.CourseImages)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t.Position)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return null;
            }

            var images = course.CourseImages?
                .Select(i => i.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList() ?? new List<string>();

            return new CourseDetailVM
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                SalesCount = course.SalesCount,
                IsFeature = course.IsFeature,
                IsNew = course.IsNew,
                TeacherName = course.Teacher?.FullName,
                TeacherImage = course.Teacher?.Image,
                TeacherPosition = course.Teacher?.Position?.Name,
                MainImage = course.CourseImages?.FirstOrDefault(i => i.IsMain)?.Name
                    ?? images.FirstOrDefault(),
                CourseImages = images
            };
        }

        public async Task<CourseAdminListVM> GetAdminPagedAsync(string? search, int page, int pageSize)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var query = _context.CourseInfos
                .Include(c => c.CourseImages)
                .Include(c => c.Teacher)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Title.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var courses = await query
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new CourseAdminListVM
            {
                Courses = courses,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<CourseInfo?> GetCourseByIdAsync(int id)
        {
            return await _context.CourseInfos
                .Include(c => c.CourseImages)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t.Position)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(CourseCreateVM courseVM)
        {
            var fileName = await ImageFileHelper.SaveAsync(courseVM.MainImage, _env.WebRootPath);

            var course = new CourseInfo
            {
                Title = courseVM.Title,
                Description = courseVM.Description,
                Price = courseVM.Price,
                SalesCount = courseVM.SalesCount,
                IsFeature = courseVM.IsFeature,
                IsNew = courseVM.IsNew,
                TeacherId = courseVM.TeacherId,
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
            var course = await _context.CourseInfos
                .Include(c => c.CourseImages)
                .FirstOrDefaultAsync(c => c.Id == courseVM.Id);

            if (course == null)
            {
                return;
            }

            if (courseVM.MainImage != null && courseVM.MainImage.Length > 0)
            {
                var fileName = await ImageFileHelper.SaveAsync(courseVM.MainImage, _env.WebRootPath);
                var mainImage = course.CourseImages.FirstOrDefault(img => img.IsMain);

                if (mainImage != null)
                {
                    ImageFileHelper.DeleteIfExists(_env.WebRootPath, mainImage.Name);
                    mainImage.Name = fileName;
                }
                else
                {
                    course.CourseImages.Add(new CourseImage { Name = fileName, IsMain = true });
                }
            }

            course.Title = courseVM.Title;
            course.Description = courseVM.Description;
            course.Price = courseVM.Price;
            course.SalesCount = courseVM.SalesCount;
            course.IsFeature = courseVM.IsFeature;
            course.IsNew = courseVM.IsNew;
            course.TeacherId = courseVM.TeacherId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.CourseInfos
                .Include(c => c.CourseImages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return;
            }

            foreach (var image in course.CourseImages)
            {
                ImageFileHelper.DeleteIfExists(_env.WebRootPath, image.Name);
            }

            _context.CourseInfos.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.CourseInfos.CountAsync();
        }

        public async Task<int> GetFeaturedCountAsync()
        {
            return await _context.CourseInfos.CountAsync(c => c.IsFeature);
        }

        public async Task<int> GetNewCountAsync()
        {
            return await _context.CourseInfos.CountAsync(c => c.IsNew);
        }

        public async Task<IEnumerable<CourseInfo>> GetRecentAsync(int take)
        {
            return await _context.CourseInfos
                .Include(c => c.Teacher)
                .Include(c => c.CourseImages)
                .OrderByDescending(c => c.Id)
                .Take(take)
                .ToListAsync();
        }
    }
}
