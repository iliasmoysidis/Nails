using Domain.Schedules.Services;
using Domain.Rosters;

namespace Application.Schedules.AddSpecialAvailability;

public sealed class AddSpecialAvailabilityContext
{
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}
