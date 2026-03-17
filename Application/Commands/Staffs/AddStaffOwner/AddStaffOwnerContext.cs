using Domain.Entities;

namespace Application.Commands.Staffs;

public sealed class AddStaffOwnerContext
{
    public Staff Staff { get; set; } = default!;
}