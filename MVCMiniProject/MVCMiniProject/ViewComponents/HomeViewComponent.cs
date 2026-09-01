using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Setting;

namespace MVCMiniProject.ViewComponents
{
    public class HomeViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        public HomeViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _settingService.GetAllUIAsync();
            return View(new SettingVM
            {
                Settings = settings
            });
        }

    }
}
