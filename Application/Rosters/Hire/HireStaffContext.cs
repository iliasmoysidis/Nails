using Domain.Rosters;
using Domain.Rosters.Services;

namespace Application.Rosters.Hire;

public sealed class HireStaffContext
{
    public Staff Staff { get; set; } = null!;
    public EmploymentCreation EmploymentCreation { get; set; } = null!;
}
