using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ISettingService
    {
        Task<Dictionary<string, string>> GetAllUIAsync();
        Task<IEnumerable<Setting>> GetAllAsync();
        Task<Setting> GetByIdAsync(int id);
        Task CreateAsync(SettingCreateVM settingVM);
        Task UpdateAsync(SettingUpdateVM settingVM);
        Task DeleteAsync(int id);
    }
}
