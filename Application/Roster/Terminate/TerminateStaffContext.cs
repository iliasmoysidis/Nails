using Domain.Roster;
using Domain.Roster.Services;

namespace Application.Roster.Terminate;

public sealed class TerminateStaffContext
{
    public Staff Staff { get; set; } = default!;
    public EmploymentTermination EmploymentTermination { get; set; } = default!;
}
