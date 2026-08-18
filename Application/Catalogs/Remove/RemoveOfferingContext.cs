using Domain.Rosters;
using Domain.Catalogs.Services;

namespace Application.Catalogs.Remove;

public sealed class RemoveOfferingContext
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferingRemoval StoreOfferingRemoval { get; set; } = default!;
}
