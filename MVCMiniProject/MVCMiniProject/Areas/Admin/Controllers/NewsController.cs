using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IWebHostEnvironment _env;

        public NewsController(INewsService newsService, IWebHostEnvironment env)
        {
            _newsService = newsService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetAllAsync();
            return View(news);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var newsItem = await _newsService.GetByIdAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateVM newsVM)
        {
            if (!ModelState.IsValid)
            {
                return View(newsVM);
            }

            await _newsService.CreateAsync(newsVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var newsItem = await _newsService.GetByIdAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            var newsVM = new NewsUpdateVM
            {
                Id = newsItem.Id,
                Date = newsItem.Date,
                Description = newsItem.Description,
                ExistingImage = newsItem.Image
            };

            return View(newsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, NewsUpdateVM newsVM)
        {
            if (id != newsVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(newsVM);
            }

            await _newsService.UpdateAsync(newsVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
