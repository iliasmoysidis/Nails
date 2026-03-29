using Domain.Entities;

namespace Application.Features.Stores.Update;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public Store Store { get; set; } = default!;
}