using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/courses/search")]
        public async Task<IActionResult> SearchApi([FromQuery] string? q)
        {
            var results = await _courseService.SearchByTitleAsync(q, 8);
            return Json(results);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? q)
        {
            var query = (q ?? string.Empty).Trim();
            var results = string.IsNullOrWhiteSpace(query)
                ? Array.Empty<MVCMiniProject.ViewModels.Courses.CourseSearchVM>()
                : await _courseService.SearchByTitleAsync(query, 50);

            return View(new MVCMiniProject.ViewModels.Courses.CourseSearchPageVM
            {
                Query = query,
                Courses = results
            });
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                var course = await _courseService.GetByIdAsync(id);
                if (course == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                return View(course);
            }
            catch
            {
                Response.StatusCode = 500;
                return View("Error");
            }
        }
    }
}
