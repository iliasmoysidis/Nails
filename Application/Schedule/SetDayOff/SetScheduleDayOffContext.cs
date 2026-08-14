using Domain.Schedule.Services;
using Domain.Roster;

namespace Application.Schedule.SetDayOff;

public sealed class SetScheduleDayOffContext
{
    public Staff Staff { get; set; } = default!;
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
}
