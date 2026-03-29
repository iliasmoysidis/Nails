using Domain.Entities;

namespace Application.Features.Offerings.Update;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreCatalog Catalog { get; set; } = default!;
}