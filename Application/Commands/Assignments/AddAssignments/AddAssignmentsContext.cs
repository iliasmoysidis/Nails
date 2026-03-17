using Domain.Entities;

namespace Application.Commands.Assignments;

public sealed class AddAssignmentsContext
{
    public Staff Staff { get; set; } = default!;
    public StoreCatalog Catalog { get; set; } = default!;
    public Domain.Entities.Assignments Assignments { get; set; } = default!;
}