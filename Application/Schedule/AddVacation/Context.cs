using Domain.Schedule.Services;
using Domain.Roster;

namespace Application.Schedule.AddVacation;

public sealed class Context
{
    public ProfessionalAvailability ProfessionalAvailability { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}
