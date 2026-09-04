using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Events;
using MVCMiniProject.ViewModels.Icons;

namespace MVCMiniProject.Services
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventUIVM>> GetAllUIAsync()
        {
            IEnumerable<EventUIVM> events = await _context.Events.Select(m => new EventUIVM
            {
                Date = m.Date,
                Month = m.Month,
                Title = m.Title,
                Description = m.Description
            }).ToListAsync();
            return events;
        }
    }
}
