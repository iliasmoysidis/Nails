using Domain.Entities;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffOwnerContext
{
    public Staff Staff { get; set; } = default!;
}