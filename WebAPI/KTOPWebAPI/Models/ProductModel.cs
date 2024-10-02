namespace KTOPWebAPI.Models
{
    public class ProductModel
    {
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? OpenDate { get; set; }
        public int HomeId { get; set; }
    }
}
