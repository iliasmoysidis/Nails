using Domain.Entities;

namespace Application.Commands.StoreCalendars;

public sealed class AddStoreCalendarSpecialHoursContext
{
    public Staff Staff { get; set; } = default!;
    public StoreCalendar StoreCalendar { get; set; } = default!;
}