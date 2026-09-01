using MVCMiniProject.ViewModels.Platforms;
using MVCMiniProject.ViewModels.Videos;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IPlatformAboutService
    {
        Task<PlatformUIVM> GetAllUIAsync();
    }
}
