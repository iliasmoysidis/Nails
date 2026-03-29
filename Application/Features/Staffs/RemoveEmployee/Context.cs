using Domain.Entities;

namespace Application.Features.Staffs.RemoveEmployee;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public Domain.Entities.Assignments Assignments { get; set; } = default!;
}