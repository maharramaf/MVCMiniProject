using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        public HeaderViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task <IViewComponentResult> InvokeAsync()
        {
            var settings = await _settingService.GetAllUIAsync();
            return View(settings);
        }
    }
}
