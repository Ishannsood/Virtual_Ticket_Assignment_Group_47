namespace VirtualEventTicketing.Models.ViewModels
{
    public class PurchaseConfirmationViewModel
    {
        public Purchase Purchase { get; set; }
        public List<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
    }
}