using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using MVCMiniProject.Models;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;
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

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(EventCreateVM eventVM)
        {
            Event eventItem = new Event
            {
                Title = eventVM.Title,
                Description = eventVM.Description,
                Date = eventVM.Date,
                Month = eventVM.Month
            };

            await _context.Events.AddAsync(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EventUpdateVM eventVM)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventVM.Id);
            if (eventItem == null) return;

            eventItem.Title = eventVM.Title;
            eventItem.Description = eventVM.Description;
            eventItem.Date = eventVM.Date;
            eventItem.Month = eventVM.Month;

            _context.Events.Update(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (eventItem == null) return;

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
        }
    }
}
