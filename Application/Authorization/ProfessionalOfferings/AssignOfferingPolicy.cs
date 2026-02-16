using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Abstractions.Repositories;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.ProfessionalOfferings;

public sealed class AssignOfferingPolicy : IAssignOfferingPolicy
{
    IRequestContext _context;
    IStaffRepository _repo;

    public AssignOfferingPolicy(
        IRequestContext context,
        IStaffRepository repo
    )
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanAssignOfferingAsync(int storeId, CancellationToken ct)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        var staff = await _repo.GetByStoreId(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        if (!staff.IsOwner(_context.ActorId))
            throw Forbidden();
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to assign offering.");
}