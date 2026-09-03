using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using MVCMiniProject.ViewModels.Events;
using MVCMiniProject.ViewModels.Icons;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventUIVM>> GetAllUIAsync();
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task CreateAsync(EventCreateVM eventVM);
        Task UpdateAsync(EventUpdateVM eventVM);
        Task DeleteAsync(int id);
    }
}
