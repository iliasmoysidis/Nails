namespace Infrastructure.Entities;

public class StoreCalendarWorkingRangeEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}