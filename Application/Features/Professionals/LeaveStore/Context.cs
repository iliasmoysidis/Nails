using Domain.Entities;

namespace Application.Features.Professionals.LeaveStore;

public sealed class Context
{
    public Staff Staff { get; set; } = default!;
}