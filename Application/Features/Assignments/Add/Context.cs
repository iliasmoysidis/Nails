using Domain.Entities;

namespace Application.Features.Assignments.Add;

public sealed class Context
{
    public StoreCapabilities StoreCapabilities { get; set; } = default!;
}
