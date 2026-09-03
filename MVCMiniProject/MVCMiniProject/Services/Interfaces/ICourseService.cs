using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Courses;
using MVCMiniProject.ViewModels.Events;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseInfoUIVM>> GetAllUIAsync();
        Task<CourseDetailVM> GetByIdAsync(int id);
        Task<IEnumerable<CourseInfo>> GetAllAsync();
        Task<CourseInfo> GetCourseByIdAsync(int id);
        Task CreateAsync(CourseCreateVM courseVM);
        Task UpdateAsync(CourseUpdateVM courseVM);
        Task DeleteAsync(int id);
    }
}
