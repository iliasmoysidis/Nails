using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Abstractions.Repositories;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.ProfessionalOfferings;

public sealed class UnassignOfferingPolicy : IUnassignOfferingPolicy
{
    IRequestContext _context;
    IStaffRepository _repo;

    public UnassignOfferingPolicy(
        IRequestContext context,
        IStaffRepository repo
    )
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanUnassignOfferingAsync(int storeId, int professionalId, CancellationToken ct)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        var staff = await _repo.GetByStoreId(storeId, ct)
            ?? throw Forbidden();

        if (!staff.IsOwner(_context.ActorId) && _context.ActorId != professionalId)
            throw Forbidden();
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to unassign offering.");
}