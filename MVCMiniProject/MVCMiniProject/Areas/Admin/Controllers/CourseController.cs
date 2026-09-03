using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ITeacherService _teacherService;
        private readonly IWebHostEnvironment _env;

        public CourseController(ICourseService courseService, ITeacherService teacherService, IWebHostEnvironment env)
        {
            _courseService = courseService;
            _teacherService = teacherService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = new SelectList(await _teacherService.GetAllAsync(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM courseVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = new SelectList(await _teacherService.GetAllAsync(), "Id", "FullName");
                return View(courseVM);
            }

            await _courseService.CreateAsync(courseVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var mainImage = course.CourseImages?.FirstOrDefault(img => img.IsMain);

            var courseVM = new CourseUpdateVM
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                SalesCount = course.SalesCount,
                IsFeature = course.IsFeature,
                IsNew = course.IsNew,
                TeacherId = course.TeacherId,
                ExistingImage = mainImage?.Name
            };

            ViewBag.Teachers = new SelectList(await _teacherService.GetAllAsync(), "Id", "FullName", course.TeacherId);
            return View(courseVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CourseUpdateVM courseVM)
        {
            if (id != courseVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Teachers = new SelectList(await _teacherService.GetAllAsync(), "Id", "FullName", courseVM.TeacherId);
                return View(courseVM);
            }

            await _courseService.UpdateAsync(courseVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
