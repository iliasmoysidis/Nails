using Domain.Entities;

namespace Application.Features.Staffs.AddEmployee;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
}