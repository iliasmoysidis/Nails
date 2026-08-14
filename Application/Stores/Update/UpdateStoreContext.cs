using Domain.Roster;
using Domain.Stores;

namespace Application.Stores.Update;

public sealed class UpdateStoreContext
{
    public Staff Staff { get; set; } = default!;
    public Store Store { get; set; } = default!;
}