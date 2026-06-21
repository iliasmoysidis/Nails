using Domain.Roster;
using Domain.Catalog.Services;

namespace Application.Catalog.Remove;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferingRemoval StoreOfferingRemoval { get; set; } = default!;
}
