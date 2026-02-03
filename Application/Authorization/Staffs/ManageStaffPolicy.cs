using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization;

public sealed class ManageStaffPolicy : IManageStaffPolicy
{
    private readonly IRequestContext _context;
    private readonly IStaffRepository _repo;

    public ManageStaffPolicy(
        IRequestContext context,
        IStaffRepository repo
    )
    {
        _context = context;
        _repo = repo;
    }

    public async Task EnsureCanManageStaffAsync(int storeId, CancellationToken ct)
    {
        if (!_context.IsProfessional) throw Forbidden();

        var staff = await _repo.GetByStoreId(storeId, ct) ?? throw Forbidden();

        if (!staff.IsOwner(_context.ActorId)) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Only store owners can manage staff.");
}