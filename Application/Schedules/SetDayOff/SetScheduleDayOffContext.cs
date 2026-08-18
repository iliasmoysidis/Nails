using Domain.Schedules.Services;
using Domain.Rosters;

namespace Application.Schedules.SetDayOff;

public sealed class SetScheduleDayOffContext
{
    public Staff Staff { get; set; } = default!;
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
}
