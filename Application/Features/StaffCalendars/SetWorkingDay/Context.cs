using Domain.Entities;

namespace Application.Features.StaffCalendars.SetWorkingDay;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreCalendar StoreCalendar { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
}