using Domain.Entities;

namespace Application.Features.StaffCalendars.AddVacation;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StaffCalendar StaffCalendar { get; set; } = default!;
}