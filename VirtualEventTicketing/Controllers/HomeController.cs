using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualEventTicketing.Data;

namespace VirtualEventTicketing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalEvents = await _context.Events.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var lowStockEvents = await _context.Events.Where(e => e.AvailableTickets < 5).CountAsync();

            ViewBag.TotalEvents = totalEvents;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.LowStockEvents = lowStockEvents;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}