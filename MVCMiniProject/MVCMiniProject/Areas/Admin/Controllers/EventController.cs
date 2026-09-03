using Microsoft.AspNetCore.Mvc;
using MVCMiniProject.Services.Interfaces;
using MVCMiniProject.ViewModels.Admin;

namespace MVCMiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllAsync();
            return View(events);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var eventItem = await _eventService.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateVM eventVM)
        {
            if (!ModelState.IsValid)
            {
                return View(eventVM);
            }

            await _eventService.CreateAsync(eventVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var eventItem = await _eventService.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var eventVM = new EventUpdateVM
            {
                Id = eventItem.Id,
                Title = eventItem.Title,
                Description = eventItem.Description,
                Date = eventItem.Date,
                Month = eventItem.Month
            };

            return View(eventVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, EventUpdateVM eventVM)
        {
            if (id != eventVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(eventVM);
            }

            await _eventService.UpdateAsync(eventVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
