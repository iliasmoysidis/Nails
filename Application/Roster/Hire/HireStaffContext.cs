using Domain.Roster;
using Domain.Roster.Services;

namespace Application.Roster.Hire;

public sealed class HireStaffContext
{
    public Staff Staff { get; set; } = null!;
    public EmploymentCreation EmploymentCreation { get; set; } = null!;
}
