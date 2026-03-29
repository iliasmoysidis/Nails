using Domain.Entities;

namespace Application.Features.Staffs.RemoveOwner;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
}