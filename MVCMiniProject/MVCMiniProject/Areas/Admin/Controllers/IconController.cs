using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IconController : Controller
    {
        private readonly IIconService _iconService;

        public IconController(IIconService iconService)
        {
            _iconService = iconService;
        }

        public async Task<IActionResult> Index()
        {
            var icons = await _iconService.GetAllAsync();
            return View(icons);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var icon = await _iconService.GetByIdAsync(id);
            if (icon == null)
            {
                return NotFound();
            }
            return View(icon);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IconCreateVM iconVM)
        {
            if (!ModelState.IsValid)
            {
                return View(iconVM);
            }

            await _iconService.CreateAsync(iconVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var icon = await _iconService.GetByIdAsync(id);
            if (icon == null)
            {
                return NotFound();
            }

            var iconVM = new IconUpdateVM
            {
                Id = icon.Id,
                Name = icon.Name
            };

            return View(iconVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, IconUpdateVM iconVM)
        {
            if (id != iconVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(iconVM);
            }

            await _iconService.UpdateAsync(iconVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _iconService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
