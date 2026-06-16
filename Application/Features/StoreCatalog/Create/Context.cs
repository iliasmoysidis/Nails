using Domain.Entities;
using Domain.Services;

namespace Application.Features.Offerings.Create;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferings StoreOfferings { get; set; } = default!;
}
