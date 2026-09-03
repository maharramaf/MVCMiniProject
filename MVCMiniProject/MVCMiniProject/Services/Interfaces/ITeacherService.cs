using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.News;
using MVCMiniProject.ViewModels.Teachers;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherUIVM>> GetAllUIAsync();
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher> GetByIdAsync(int id);
        Task CreateAsync(TeacherCreateVM teacherVM);
        Task UpdateAsync(TeacherUpdateVM teacherVM);
        Task DeleteAsync(int id);
    }
}
