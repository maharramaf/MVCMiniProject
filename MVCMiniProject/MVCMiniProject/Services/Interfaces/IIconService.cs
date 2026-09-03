using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Icons;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IIconService
    {
        Task<IEnumerable<IconUIVM>> GetAllUIAsync();
        Task<IEnumerable<Icon>> GetAllAsync();
        Task<Icon> GetByIdAsync(int id);
        Task CreateAsync(IconCreateVM iconVM);
        Task UpdateAsync(IconUpdateVM iconVM);
        Task DeleteAsync(int id);
    }
}
