using Domain.Roster;
using Domain.Stores.Services;

namespace Application.Assignments.Add;

public sealed class AddAssignmentsContext
{
    public StoreAssignments StoreAssignments { get; set; } = default!;

    public Staff Staff { get; set; } = null!;
}
