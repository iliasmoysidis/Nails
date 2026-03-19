using Domain.Entities;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreContext
{
    public Staff Staff { get; set; } = default!;
}