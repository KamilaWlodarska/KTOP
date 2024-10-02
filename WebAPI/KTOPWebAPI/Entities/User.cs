namespace KTOPWebAPI.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime LastModified { get; set; }

    public virtual ICollection<Home> Homes { get; set; } = new List<Home>();

    public virtual ICollection<UsersHome> UsersHomes { get; set; } = new List<UsersHome>();
}
