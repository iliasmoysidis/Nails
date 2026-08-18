using Domain.Rosters;
using Domain.Rosters.Services;

namespace Application.Rosters.Terminate;

public sealed class TerminateStaffContext
{
    public Staff Staff { get; set; } = default!;
    public EmploymentTermination EmploymentTermination { get; set; } = default!;
}
