using Domain.Entities;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingContext
{
    public Staff Staff { get; set; } = default!;
    public StoreCatalog Catalog { get; set; } = default!;
}