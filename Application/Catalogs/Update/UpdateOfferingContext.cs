using Domain.Rosters;
using Domain.Catalogs.Services;

namespace Application.Catalogs.Update;

public sealed class UpdateOfferingContext
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferings StoreOfferings { get; set; } = default!;
}
