using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Staffs;

public sealed class AddStaffEmployeeLoader
    : IRequestContextLoader<
        AddStaffEmployeeCommand,
        AddStaffEmployeeContext>
{
    private readonly IStaffRepository _repo;

    public AddStaffEmployeeLoader(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        AddStaffEmployeeCommand command,
        AddStaffEmployeeContext ctx,
        CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Staff = staff;
    }
}