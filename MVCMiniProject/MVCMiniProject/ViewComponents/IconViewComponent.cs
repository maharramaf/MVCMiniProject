using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.ViewComponents
{
    public class IconViewComponent : ViewComponent
    {
        private readonly IIconService _iconService;
        public IconViewComponent(IIconService iconService)
        {
            _iconService = iconService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var icon = await _iconService.GetAllUIAsync();
            return View(icon);
        }
    }
}
