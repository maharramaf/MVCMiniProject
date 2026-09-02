using MVCMiniProject.ViewModels.Courses;
using MVCMiniProject.ViewModels.Events;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseInfoUIVM>> GetAllUIAsync();
        Task<CourseDetailVM> GetByIdAsync(int id);
    }
}
