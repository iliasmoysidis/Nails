using Nails.Enums;

namespace Nails.Models
{
    public class StoreManager
    {
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public StoreRole Role { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } = null;

        public Store Store { get; set; } = null!;
        public User User { get; set; } = null!;

    }
}