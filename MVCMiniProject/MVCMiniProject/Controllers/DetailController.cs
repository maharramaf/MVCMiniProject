using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.Controllers
{
    public class DetailController : Controller
    {
        private readonly ICourseService _courseService;

        public DetailController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
    }
}
