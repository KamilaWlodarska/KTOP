namespace KTOPWebAPI.Entities;

public partial class UsersHome
{
    public int UsersHomesId { get; set; }

    public int UserId { get; set; }

    public int HomeId { get; set; }

    public virtual Home Home { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
