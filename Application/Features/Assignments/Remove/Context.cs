using Domain.Entities;
using Domain.Services;

namespace Application.Features.Assignments.Remove;

public sealed class Context
{
    public StoreAssignments StoreAssignments { get; set; } = default!;
}
