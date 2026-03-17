using Domain.Entities;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarWorkingDayContext
{
    public Staff Staff { get; set; } = default!;
    public StoreCalendar StoreCalendar { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
}