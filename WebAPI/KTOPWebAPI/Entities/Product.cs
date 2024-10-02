namespace KTOPWebAPI.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public DateTime? OpenDate { get; set; }

    public int HomeId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Home Home { get; set; } = null!;
}
