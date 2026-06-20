using Domain.Roster;
using Domain.Roster.Services;

namespace Application.Features.Staffs.Hire;

public sealed class Context
{
    public Staff Staff { get; set; } = null!;
    public EmploymentCreation EmploymentCreation { get; set; } = null!;
}
