namespace Nails.Models
{
    public class StoreOperatingHours
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsClosed { get; set; } = false;

        public Store Store { get; set; } = null!;
    }
}