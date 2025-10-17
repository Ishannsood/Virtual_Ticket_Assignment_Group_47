namespace VirtualEventTicketing.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int EventId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerTicket { get; set; }
        
        // Navigation properties
        public Purchase Purchase { get; set; }
        public Event Event { get; set; }
    }
}