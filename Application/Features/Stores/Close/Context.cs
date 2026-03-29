using Domain.Entities;

namespace Application.Features.Stores.Close;

public sealed class Context
{
    public Store Store { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}