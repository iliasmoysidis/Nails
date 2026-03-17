using Domain.Entities;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffEmployeeContext
{
    public Staff Staff { get; set; } = default!;
    public Domain.Entities.Assignments Assignments { get; set; } = default!;
}