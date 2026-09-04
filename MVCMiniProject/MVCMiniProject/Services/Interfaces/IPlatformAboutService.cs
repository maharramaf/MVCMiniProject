using MVCMiniProject.ViewModels.Platforms;
using MVCMiniProject.ViewModels.Videos;
using System.Threading.Tasks;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IPlatformAboutService
    {
        Task<PlatformUIVM> GetAllUIAsync();
    }
}
