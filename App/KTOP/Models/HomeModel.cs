namespace KTOP.Models
{
    public class HomeModel
    {
        public int HomeId { get; set; }
        public string HomeName { get; set; } = null!;
        public int OwnerId { get; set; }
        public virtual UserModel Owner { get; set; } = null!;
    }
}
