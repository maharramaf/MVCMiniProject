using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Events;
using MVCMiniProject.ViewModels.Icons;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventUIVM>> GetAllUIAsync();
    }
}
