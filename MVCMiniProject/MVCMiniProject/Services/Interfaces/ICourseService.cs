using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Courses;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseInfoUIVM>> GetAllUIAsync();
        Task<IReadOnlyList<CourseSearchVM>> SearchByTitleAsync(string? query, int take = 8);
        Task<CourseDetailVM?> GetByIdAsync(int id);
        Task<CourseAdminListVM> GetAdminPagedAsync(string? search, int page, int pageSize);
        Task<CourseInfo?> GetCourseByIdAsync(int id);
        Task CreateAsync(CourseCreateVM courseVM);
        Task UpdateAsync(CourseUpdateVM courseVM);
        Task DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task<int> GetFeaturedCountAsync();
        Task<int> GetNewCountAsync();
        Task<IEnumerable<CourseInfo>> GetRecentAsync(int take);
    }
}
