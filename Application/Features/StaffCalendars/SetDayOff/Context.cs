using Domain.Entities;
using Domain.Services;

namespace Application.Features.StaffCalendars.SetDayOff;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
}
