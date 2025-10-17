namespace VirtualEventTicketing.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDateTime { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableTickets { get; set; }
        public int CategoryId { get; set; }
        
        // Navigation properties
        public Category Category { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
    }
}