using Domain.Schedule.Services;
using Domain.Roster;

namespace Application.Features.StaffCalendars.RemoveException;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
}
