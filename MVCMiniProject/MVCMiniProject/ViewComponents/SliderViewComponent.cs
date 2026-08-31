using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ISliderService _sliderService;
        public SliderViewComponent(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slider = await _sliderService.GetAllUIAsync();
            return View(slider);
        }
    }
}
