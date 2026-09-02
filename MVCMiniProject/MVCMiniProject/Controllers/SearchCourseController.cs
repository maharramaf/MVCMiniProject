using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.Controllers
{
    public class SearchCourseController : Controller
    {
        private readonly ICourseService _courseService;

        public SearchCourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [ActionName("Search")]
        public async Task<IActionResult> SearchGet(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return RedirectToAction("Index", "Home");
            }
            
            var allCourses = await _courseService.GetAllUIAsync();
            var courses = allCourses.Where(c => c.Title.ToLower().Contains(searchQuery.ToLower().Trim())).ToList();

            if (courses.Count == 1)
            {
               
                return RedirectToAction("Index", "Detail", new { id = courses.First().Id });
            }
            else if (courses.Count > 1)
            {
               
                ViewBag.SearchQuery = searchQuery;
                ViewBag.ResultCount = courses.Count;
                return View("SearchResults", courses);
            }
            else
            {
                // Heç bir kurs tapılmayıb
                ViewBag.SearchQuery = searchQuery;
                ViewBag.Message = $"'{searchQuery}' adli kurs tapilmadi";
                return View("NotFound");
            }
        }

        [HttpPost]
        [ActionName("Search")]
        public async Task<IActionResult> SearchPost(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                ViewBag.SearchQuery = "";
                ViewBag.Message = "Zəhmət olmasa axtarış üçün kurs adı daxil edin";
                return View("NotFound");
            }

            var allCourses = await _courseService.GetAllUIAsync();
            var courses = allCourses.Where(c => c.Title.ToLower().Contains(searchQuery.ToLower().Trim())).ToList();

            if (courses.Count == 1)
            {
                return RedirectToAction("Index", "Detail", new { id = courses.First().Id });
            }
            else if (courses.Count > 1)
            {
                ViewBag.SearchQuery = searchQuery;
                ViewBag.ResultCount = courses.Count;
                return View("SearchResults", courses);
            }
            else
            {
                ViewBag.SearchQuery = searchQuery;
                ViewBag.Message = $"'{searchQuery}' adli kurs tapilmadi";
                return View("NotFound");
            }
        }

        public IActionResult NotFound()
        {
            return View();
        }
    }
}
