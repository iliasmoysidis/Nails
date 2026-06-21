namespace Infrastructure.Schedule;

public class StaffCalendarExceptionEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProfessionalId { get; set; }
    public DateOnly Date { get; set; }
    public bool IsDayOff { get; set; }
    public TimeSpan? Start { get; set; }
    public TimeSpan? End { get; set; }
}