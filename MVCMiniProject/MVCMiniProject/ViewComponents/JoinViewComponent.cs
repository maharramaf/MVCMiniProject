using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Setting;

namespace MVCMiniProject.ViewComponents
{
    public class JoinViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        public JoinViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var joins = await _settingService.GetAllUIAsync();
            return View(new SettingVM
            {
                Settings = joins
            });
        }
    }
}
