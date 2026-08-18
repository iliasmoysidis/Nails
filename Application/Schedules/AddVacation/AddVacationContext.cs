using Domain.Schedules.Services;
using Domain.Rosters;

namespace Application.Schedules.AddVacation;

public sealed class AddVacationContext
{
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}
