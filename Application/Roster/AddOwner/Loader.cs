using Application.Roster.Common.Repositories;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Roster.AddOwner;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStaffRepository _repo;

    public Loader(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct
    )
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Staff = staff;
    }
}