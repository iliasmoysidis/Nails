using Domain.Entities;

namespace Application.Features.Assignments.Add;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
    public StoreCatalog Catalog { get; set; } = default!;
    public Domain.Entities.Assignments Assignments { get; set; } = default!;
}