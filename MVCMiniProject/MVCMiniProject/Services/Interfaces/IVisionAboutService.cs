using MVCMiniProject.ViewModels.Videos;
using MVCMiniProject.ViewModels.Visions;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IVisionAboutService
    {
        Task<VisionUIVM> GetAllUIAsync();
    }
}
