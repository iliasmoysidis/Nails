using Domain.Schedule.Services;
using Domain.Roster;

namespace Application.Schedule.RemoveException;

public sealed class RemoveScheduleExceptionContext
{
    public Staff Staff { get; set; } = default!;
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
}
