using Domain.Entities;
using Domain.Services;

namespace Application.Features.Offerings.Delete;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreOfferingRemoval StoreOfferingRemoval { get; set; } = default!;
}
