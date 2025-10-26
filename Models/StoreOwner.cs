namespace Nails.Models
{
    public class StoreOwner
    {
        public int UserId { get; set; }
        public int StoreId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } = null;

        public User User { get; set; } = null!;
        public Store Store { get; set; } = null!;
    }
}