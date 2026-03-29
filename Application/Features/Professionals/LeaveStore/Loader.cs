using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreLoader
    : IRequestContextLoader<LeaveProfessionalStoreCommand, LeaveProfessionalStoreContext>
{
    private readonly IStaffRepository _repo;


    public LeaveProfessionalStoreLoader(
        IStaffRepository repo
    )
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        LeaveProfessionalStoreCommand command,
        LeaveProfessionalStoreContext ctx,
        CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        ctx.Staff = staff;
    }
}