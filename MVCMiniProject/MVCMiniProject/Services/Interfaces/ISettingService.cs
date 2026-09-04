using MVCMiniProject.Models;

namespace MVCMiniProject.Services.Interfaces
{
    public interface ISettingService
    {
        Task<Dictionary<string, string>> GetAllUIAsync();
    }
}
