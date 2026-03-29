using Domain.Entities;

namespace Application.Features.StaffCalendars.AddSpecialAvailability;

public sealed class Context
{
    public StoreCalendar StoreCalendar { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}