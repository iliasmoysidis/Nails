using Domain.Professionals;
using Domain.Stores.Services;

namespace Application.Assignments.Remove;

public sealed class Context
{
    public StoreAssignments StoreAssignments { get; set; } = default!;

    public Professional Professional {get; set;} = null!;
}
