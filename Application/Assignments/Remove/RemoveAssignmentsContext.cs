using Domain.Roster;
using Domain.Stores.Services;

namespace Application.Assignments.Remove;

public sealed class RemoveAssignmentsContext
{
    public StoreAssignments StoreAssignments { get; set; } = default!;

    public Staff Staff { get; set; } = null!;
}
