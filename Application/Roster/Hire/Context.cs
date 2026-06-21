using Domain.Roster;
using Domain.Roster.Services;

namespace Application.Roster.Hire;

public sealed class Context
{
    public Staff Staff { get; set; } = null!;
    public EmploymentCreation EmploymentCreation { get; set; } = null!;
}
