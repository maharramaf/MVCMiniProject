using MVCMiniProject.ViewModels.Videos;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IVideoService
    {
        Task<VideoUIVM> GetAllUIAsync();
    }
}
