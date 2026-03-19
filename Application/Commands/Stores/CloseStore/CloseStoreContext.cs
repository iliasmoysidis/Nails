using Domain.Entities;

namespace Application.Commands.Stores;

public sealed class CloseStoreContext
{
    public Store Store { get; set; } = default!;
    public Staff Staff { get; set; } = default!;
}