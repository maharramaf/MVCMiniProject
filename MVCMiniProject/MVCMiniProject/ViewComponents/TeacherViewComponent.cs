using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services;
using MVCMiniProject.Services.Interfaces;

namespace MVCMiniProject.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly ITeacherService _teacherService;
        public TeacherViewComponent(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teacher = await _teacherService.GetAllUIAsync();
            return View(teacher);
        }
    }
}
