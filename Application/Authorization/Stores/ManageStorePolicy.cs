using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Stores;

public sealed class ManageStorePolicy : IManageStorePolicy
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public ManageStorePolicy(IRequestContext context, IStaffRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanManageAsync(int storeId, CancellationToken ct)
    {
        if (!_context.IsProfessional) throw Forbidden();

        var staff = await _repo.GetByStoreId(storeId, ct) ?? throw Forbidden();

        if (!staff.IsOwner(_context.ActorId)) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Only a store owner can manage store details.");
}