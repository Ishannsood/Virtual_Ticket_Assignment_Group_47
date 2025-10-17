namespace VirtualEventTicketing.Models.ViewModels
{
    public class CreatePurchaseViewModel
    {
        public int EventId { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public int Quantity { get; set; }
        
        public Event? Event { get; set; }
    }
}