using Domain.Entities;

namespace Application.Features.StoreCalendars.AddSpecialHours;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreCalendar StoreCalendar { get; set; } = default!;
}