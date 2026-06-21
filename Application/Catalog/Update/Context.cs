using Domain.Roster;
using Domain.Catalog.Services;

namespace Application.Catalog.Update;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferings StoreOfferings { get; set; } = default!;
}
