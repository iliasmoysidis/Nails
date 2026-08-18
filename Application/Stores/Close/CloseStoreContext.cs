using Domain.Rosters;
using Domain.Stores.Services;

namespace Application.Stores.Close;

public sealed class CloseStoreContext
{
    public StoreClosure StoreClosure { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
}
