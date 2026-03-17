using Domain.Entities;

namespace Application.Commands.Stores;

public sealed class UpdateStoreContext
{
    public Staff Staff { get; set; } = default!;
    public Store Store { get; set; } = default!;
}