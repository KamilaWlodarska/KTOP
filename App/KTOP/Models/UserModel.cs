namespace KTOP.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public DateTime LastModified { get; set; }
    }
}
