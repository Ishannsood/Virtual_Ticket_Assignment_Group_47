// PurchasesController.cs - Fixed DateTime Issue
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualEventTicketing.Data;
using VirtualEventTicketing.Models;
using VirtualEventTicketing.Models.ViewModels;

namespace VirtualEventTicketing.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Purchases/Create
        public async Task<IActionResult> Create(int eventId)
        {
            var eventObj = await _context.Events.FindAsync(eventId);
            if (eventObj == null)
                return NotFound();

            var viewModel = new CreatePurchaseViewModel
            {
                EventId = eventId,
                Event = eventObj
            };

            return View(viewModel);
        }

        // POST: Purchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePurchaseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Event = await _context.Events.FindAsync(viewModel.EventId);
                return View(viewModel);
            }

            var eventObj = await _context.Events.FindAsync(viewModel.EventId);
            if (eventObj == null)
                return NotFound();

            // Check ticket availability
            if (viewModel.Quantity > eventObj.AvailableTickets)
            {
                ModelState.AddModelError("Quantity", "Not enough tickets available");
                viewModel.Event = eventObj;
                return View(viewModel);
            }

            // Validate quantity is positive
            if (viewModel.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Quantity must be greater than 0");
                viewModel.Event = eventObj;
                return View(viewModel);
            }

            // Create purchase - USE DateTime.UtcNow for PostgreSQL
            var purchase = new Purchase
            {
                GuestName = viewModel.GuestName.Trim(),
                GuestEmail = viewModel.GuestEmail.Trim(),
                PurchaseDate = DateTime.UtcNow,  // ✅ DateTime.UtcNow for UTC time
                TotalCost = viewModel.Quantity * eventObj.TicketPrice
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            // Create purchase item
            var purchaseItem = new PurchaseItem
            {
                PurchaseId = purchase.Id,
                EventId = viewModel.EventId,
                Quantity = viewModel.Quantity,
                PricePerTicket = eventObj.TicketPrice
            };

            _context.PurchaseItems.Add(purchaseItem);

            // Update available tickets
            eventObj.AvailableTickets -= viewModel.Quantity;
            _context.Events.Update(eventObj);
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirm", new { purchaseId = purchase.Id });
        }

        // GET: Purchases/Confirm/5
        public async Task<IActionResult> Confirm(int purchaseId)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Items)
                .ThenInclude(pi => pi.Event)
                .FirstOrDefaultAsync(p => p.Id == purchaseId);

            if (purchase == null)
                return NotFound();

            var viewModel = new PurchaseConfirmationViewModel
            {
                Purchase = purchase,
                Items = purchase.Items.ToList()
            };

            return View(viewModel);
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int Id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Items)
                .ThenInclude(pi => pi.Event)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (purchase == null)
                return NotFound();

            var viewModel = new PurchaseConfirmationViewModel
            {
                Purchase = purchase,
                Items = purchase.Items.ToList()
            };

            return View(viewModel);
        }

        // GET: Purchases/List - View all purchases
        public async Task<IActionResult> List()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Items)
                .ThenInclude(pi => pi.Event)
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();

            return View(purchases);
        }
    }
}