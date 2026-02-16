using Application.Abstractions.Policies.Offerings;
using Application.Abstractions.Repositories;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Offerings;

public sealed class ManageOfferingPolicy : IManageOfferingPolicy
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public ManageOfferingPolicy(IRequestContext context, IStaffRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanManageAsync(int storeId, CancellationToken ct)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        var staff = await _repo.GetByStoreId(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        if (!staff.IsOwner(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Only store owners can create offerings.");
}