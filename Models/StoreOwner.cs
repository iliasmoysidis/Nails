namespace Nails.Models
{
    public class StoreOwner
    {
        public int UserId { get; set; }
        public int StoreId { get; set; }

        public User User { get; set; } = null!;
        public Store Store { get; set; } = null!;
    }
}