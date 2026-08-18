using Domain.Rosters;

namespace Application.Rosters.AddOwner;

public sealed class AddStoreOwnerContext
{
    public Staff Staff { get; set; } = default!;
}