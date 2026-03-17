using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffOwnerLoader
    : IRequestContextLoader<
        RemoveStaffOwnerCommand,
        RemoveStaffOwnerContext>
{
    private readonly IStaffRepository _repo;

    public RemoveStaffOwnerLoader(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        RemoveStaffOwnerCommand command,
        RemoveStaffOwnerContext ctx,
        CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Staff = staff;
    }
}