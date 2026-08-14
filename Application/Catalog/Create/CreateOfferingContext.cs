using Domain.Roster;
using Domain.Catalog.Services;

namespace Application.Catalog.Create;

public sealed class CreateOfferingContext
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferings StoreOfferings { get; set; } = default!;
}
