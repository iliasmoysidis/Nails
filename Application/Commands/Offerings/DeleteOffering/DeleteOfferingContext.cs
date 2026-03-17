using Domain.Entities;

namespace Application.Commands.Offerings;

public sealed class DeleteOfferingContext
{
    public Staff Staff { get; set; } = default!;
    public StoreCatalog Catalog { get; set; } = default!;
    public Domain.Entities.Assignments Assignments { get; set; } = default!;
}