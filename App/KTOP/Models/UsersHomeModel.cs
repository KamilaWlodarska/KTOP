namespace KTOP.Models
{
    public class UsersHomeModel
    {
        public int UsersHomesId { get; set; }
        public int UserId { get; set; }
        public int HomeId { get; set; }
        public virtual HomeModel Home { get; set; } = null!;
        public virtual UserModel User { get; set; } = null!;
        public string UserName => User?.UserName ?? string.Empty;
    }
}
