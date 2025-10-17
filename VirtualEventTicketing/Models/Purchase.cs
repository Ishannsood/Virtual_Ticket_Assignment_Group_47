namespace VirtualEventTicketing.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalCost { get; set; }
        
        // Navigation property
        public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
    }
}