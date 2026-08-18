using Domain.Rosters;
using Domain.Catalogs.Services;

namespace Application.Catalogs.Create;

public sealed class CreateOfferingContext
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferings StoreOfferings { get; set; } = default!;
}
