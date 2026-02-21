using Application.Abstractions.Policies.Professionals;
using Application.Abstractions.Repositories;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Professionals;

public sealed class LeaveStorePolicy : ILeaveStorePolicy
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public LeaveStorePolicy(
        IRequestContext context,
        IStaffRepository repo
    )
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanLeaveAsync(int storeId, CancellationToken ct)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        var staff = await _repo.GetByStoreId(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        if (!staff.IsStaff(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to leave store.");
}