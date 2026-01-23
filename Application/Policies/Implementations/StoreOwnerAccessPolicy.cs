
using Application.Exceptions;
using Application.Policies.Interfaces;
using Application.Repositories;

namespace Application.Implementation.Policies;

public sealed class StoreOwnerAccessPolicy : IStoreOwnerAccessPolicy
{
    private readonly IStaffRepository _repo;

    public StoreOwnerAccessPolicy(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task EnsureIsOwnerAsync(int userId, int storeId, CancellationToken ct)
    {
        var staff = await _repo.GetStaffAsync(storeId, ct);

        if (!staff.IsOwner(userId))
            throw new ApplicationLayerException("Only the store owner can access store appointments.");
    }
}