using Domain.Entities;

namespace Application.Features.Assignments.Remove;

public sealed class Context
{
    public StoreCapabilities StoreCapabilities { get; set; } = default!;
}
