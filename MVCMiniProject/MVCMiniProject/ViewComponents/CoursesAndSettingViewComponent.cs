using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.CoursesAndSetting;


namespace MVCMiniProject.ViewComponents
{
    public class CoursesAndSettingViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly ICourseService _courseService;
        public CoursesAndSettingViewComponent(ISettingService settingService,
                                              ICourseService courseService)
        {
            _settingService = settingService;
            _courseService = courseService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _settingService.GetAllUIAsync();
            var course = await _courseService.GetAllUIAsync();
            return View(new CoursesAndSettingVM
            {
                Settings = setting,
                CourseInfos = course
            });
        }
    }
}
