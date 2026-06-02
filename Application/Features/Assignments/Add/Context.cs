using Domain.Entities;
using Domain.Services;

namespace Application.Features.Assignments.Add;

public sealed class Context
{
    public StoreAssignments StoreAssignments { get; set; } = default!;
}
