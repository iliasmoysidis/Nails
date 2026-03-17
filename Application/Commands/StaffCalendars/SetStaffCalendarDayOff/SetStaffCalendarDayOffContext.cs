using Domain.Entities;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarDayOffContext
{
    public Staff Staff { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
}