using Domain.Entities;

namespace Application.Features.StaffCalendars.RemoveException;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
}