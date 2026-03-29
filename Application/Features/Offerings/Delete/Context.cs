using Domain.Entities;

namespace Application.Features.Offerings.Delete;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreCatalog Catalog { get; set; } = default!;
    public Domain.Entities.Assignments Assignments { get; set; } = default!;
}