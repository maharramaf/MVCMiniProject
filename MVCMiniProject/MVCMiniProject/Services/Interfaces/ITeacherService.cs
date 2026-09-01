using MVCMiniProject.ViewModels.News;
using MVCMiniProject.ViewModels.Teachers;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherUIVM>> GetAllUIAsync();
    }
}
