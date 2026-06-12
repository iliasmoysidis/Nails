using Domain.Entities;

namespace Application.Features.Offerings.Create;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreCatalog Catalog { get; set; } = default!;
}