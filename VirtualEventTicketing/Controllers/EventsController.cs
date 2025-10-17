using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualEventTicketing.Data;
using VirtualEventTicketing.Models;
using VirtualEventTicketing.Models.ViewModels;

namespace VirtualEventTicketing.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET action - displays the create form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateEventViewModel
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(model);
        }

        // POST action - handles form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _context.Categories.ToListAsync();
                return View(model);
            }

            var @event = new Event
            {
                Title = model.Title,
                CategoryId = model.CategoryId,
                EventDateTime = model.EventDateTime.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(model.EventDateTime, DateTimeKind.Utc)
                    : model.EventDateTime,
                TicketPrice = model.TicketPrice,
                AvailableTickets = model.AvailableTickets
            };;

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        // GET: Events/Index - List all events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .OrderByDescending(e => e.EventDateTime)
                .ToListAsync();
            
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var @event = await _context.Events
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
                return NotFound();

            return View(@event);
       }
        // POST: Events/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var @event = await _context.Events.FindAsync(id);
    
            if (@event == null)
                return NotFound();

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
    
            return RedirectToAction("Index");
        }
    }
}