using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Teachers;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherUIVM>> GetAllUIAsync();
        Task<IEnumerable<Teacher>> GetAllAsync();
    }
}
