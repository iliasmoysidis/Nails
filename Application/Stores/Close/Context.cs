using Domain.Roster;
using Domain.Stores.Services;

namespace Application.Stores.Close;

public sealed class Context
{
    public StoreClosure StoreClosure { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
}
