using Microsoft.EntityFrameworkCore;
using VirtualEventTicketing.Models;

namespace VirtualEventTicketing.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Webinar", Description = "Online seminars and training" },
                new Category { Id = 2, Name = "Concert", Description = "Virtual music concerts" },
                new Category { Id = 3, Name = "Workshop", Description = "Interactive workshops" },
                new Category { Id = 4, Name = "Conference", Description = "Professional conferences" }
            );

            // Seed Events - Use UTC DateTime
            modelBuilder.Entity<Event>().HasData(
                new Event 
                { 
                    Id = 1, 
                    Title = "Advanced C# Programming", 
                    EventDateTime = new DateTime(2025, 10, 23, 14, 0, 0, DateTimeKind.Utc), 
                    TicketPrice = 49.99m, 
                    AvailableTickets = 100, 
                    CategoryId = 1 
                },
                new Event 
                { 
                    Id = 2, 
                    Title = "Virtual Jazz Night", 
                    EventDateTime = new DateTime(2025, 10, 30, 19, 0, 0, DateTimeKind.Utc), 
                    TicketPrice = 29.99m, 
                    AvailableTickets = 3, 
                    CategoryId = 2 
                },
                new Event 
                { 
                    Id = 3, 
                    Title = "Web Development Workshop", 
                    EventDateTime = new DateTime(2025, 10, 21, 10, 0, 0, DateTimeKind.Utc), 
                    TicketPrice = 39.99m, 
                    AvailableTickets = 50, 
                    CategoryId = 3 
                },
                new Event 
                { 
                    Id = 4, 
                    Title = "Tech Summit 2025", 
                    EventDateTime = new DateTime(2025, 11, 6, 9, 0, 0, DateTimeKind.Utc), 
                    TicketPrice = 99.99m, 
                    AvailableTickets = 0, 
                    CategoryId = 4 
                }
            );

            // Relationships
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Purchase)
                .WithMany(p => p.Items)
                .HasForeignKey(pi => pi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Event)
                .WithMany(e => e.PurchaseItems)
                .HasForeignKey(pi => pi.EventId);
        }
    }
}