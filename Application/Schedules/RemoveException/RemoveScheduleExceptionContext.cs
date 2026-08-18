using Domain.Schedules.Services;
using Domain.Rosters;

namespace Application.Schedules.RemoveException;

public sealed class RemoveScheduleExceptionContext
{
    public Staff Staff { get; set; } = default!;
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
}
