using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.CoursesAndSetting;
using MVCMiniProject.ViewModels.SettingAndCourse;

namespace MVCMiniProject.ViewComponents
{
    public class SettingAndCourseViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly ICourseService _courseService;
        public SettingAndCourseViewComponent(ISettingService settingService,
                                              ICourseService courseService)
        {
            _settingService = settingService;
            _courseService = courseService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _settingService.GetAllUIAsync();
            var course = await _courseService.GetAllUIAsync();
            return View(new SettingAndCourseVM
            {
                Settings = setting,
                CourseInfos = course
            });
        }
    }
}
