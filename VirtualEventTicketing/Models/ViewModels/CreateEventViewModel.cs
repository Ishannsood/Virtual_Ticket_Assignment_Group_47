namespace VirtualEventTicketing.Models.ViewModels
{
    public class CreateEventViewModel
    {
        public string Title { get; set; }
        public DateTime EventDateTime { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableTickets { get; set; }
        public int CategoryId { get; set; }
        
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}