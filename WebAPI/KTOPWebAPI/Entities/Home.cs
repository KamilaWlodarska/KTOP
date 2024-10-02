namespace KTOPWebAPI.Entities;

public partial class Home
{
    public int HomeId { get; set; }

    public string HomeName { get; set; } = null!;

    public int OwnerId { get; set; }

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<UsersHome> UsersHomes { get; set; } = new List<UsersHome>();
}
