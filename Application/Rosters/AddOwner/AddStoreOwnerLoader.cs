using Application.Rosters.Common.Repositories;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Rosters.AddOwner;

public sealed class AddStoreOwnerLoader
    : IRequestContextLoader<
        AddStoreOwnerCommand,
        AddStoreOwnerContext>
{
    private readonly IStaffRepository _repo;

    public AddStoreOwnerLoader(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        AddStoreOwnerCommand command,
        AddStoreOwnerContext ctx,
        CancellationToken ct
    )
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Staff = staff;
    }
}