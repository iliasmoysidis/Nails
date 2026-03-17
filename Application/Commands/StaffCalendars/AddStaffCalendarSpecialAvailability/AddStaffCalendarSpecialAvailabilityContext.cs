using Domain.Entities;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarSpecialAvailabilityContext
{
    public StoreCalendar StoreCalendar { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}