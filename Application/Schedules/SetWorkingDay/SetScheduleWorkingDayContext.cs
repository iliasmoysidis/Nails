using Domain.Schedules.Services;
using Domain.Rosters;

namespace Application.Schedules.SetWorkingDay;

public sealed class SetScheduleWorkingDayContext
{
    public Staff Staff { get; set; } = default!;
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
}
