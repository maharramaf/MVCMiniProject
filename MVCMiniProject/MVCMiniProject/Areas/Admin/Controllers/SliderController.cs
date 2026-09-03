using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IWebHostEnvironment _env;

        public SliderController(ISliderService sliderService, IWebHostEnvironment env)
        {
            _sliderService = sliderService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllAsync();
            return View(sliders);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM sliderVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderVM);
            }

            await _sliderService.CreateAsync(sliderVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            var sliderVM = new SliderUpdateVM
            {
                Id = slider.Id,
                Title = slider.Name,
                Description = slider.Description,
                ExistingImage = slider.Image
            };

            return View(sliderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SliderUpdateVM sliderVM)
        {
            if (id != sliderVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(sliderVM);
            }

            await _sliderService.UpdateAsync(sliderVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
