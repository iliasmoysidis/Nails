using Domain.Roster;
using Domain.Roster.Services;

namespace Application.Roster.Terminate;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public EmploymentTermination EmploymentTermination { get; set; } = default!;
}
