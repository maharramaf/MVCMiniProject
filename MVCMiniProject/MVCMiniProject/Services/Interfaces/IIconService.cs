using MVCMiniProject.ViewModels.Icons;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IIconService
    {
        Task<IEnumerable<IconUIVM>> GetAllUIAsync();
    }
}
