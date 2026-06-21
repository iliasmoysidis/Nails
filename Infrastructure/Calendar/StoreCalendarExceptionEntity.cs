namespace Infrastructure.Calendar;

public class StoreCalendarExceptionEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public DateOnly Date { get; set; }
    public bool IsDayOff { get; set; }
    public TimeSpan? Start { get; set; }
    public TimeSpan? End { get; set; }
}