using Domain.Entities;

namespace Application.Commands.Staffs;

public sealed class AddStaffEmployeeContext
{
    public Staff Staff { get; set; } = default!;
}