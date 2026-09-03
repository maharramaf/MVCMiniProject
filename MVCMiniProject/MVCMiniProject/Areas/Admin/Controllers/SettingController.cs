using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _settingService.GetAllAsync();
            return View(settings);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var setting = await _settingService.GetByIdAsync(id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingCreateVM settingVM)
        {
            if (!ModelState.IsValid)
            {
                return View(settingVM);
            }

            await _settingService.CreateAsync(settingVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var setting = await _settingService.GetByIdAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            var settingVM = new SettingUpdateVM
            {
                Id = setting.Id,
                Key = setting.Key,
                Value = setting.Value
            };

            return View(settingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SettingUpdateVM settingVM)
        {
            if (id != settingVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(settingVM);
            }

            await _settingService.UpdateAsync(settingVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _settingService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
