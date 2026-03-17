using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Staffs;

public sealed class AddStaffOwnerLoader
    : IRequestContextLoader<
        AddStaffOwnerCommand,
        AddStaffOwnerContext>
{
    private readonly IStaffRepository _repo;

    public AddStaffOwnerLoader(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        AddStaffOwnerCommand command,
        AddStaffOwnerContext ctx,
        CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Staff = staff;
    }
}