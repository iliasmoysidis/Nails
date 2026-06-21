namespace Infrastructure.Schedule;

public class StaffCalendarWorkingRangeEntity
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public int ProfessionalId { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}