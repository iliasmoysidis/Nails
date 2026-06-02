using Domain.Entities;
using Domain.Services;

namespace Application.Features.Staffs.TerminateEmployment;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public EmploymentTermination EmploymentTermination { get; set; } = default!;
}
