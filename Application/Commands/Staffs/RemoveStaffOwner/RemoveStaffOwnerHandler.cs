using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffOwnerHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffRepository _repo;
    private readonly IClock _clock;


    public RemoveStaffOwnerHandler(
        AuthorizationGuard auth,
        IStaffRepository repo,
        IClock clock
    )
    {
        _auth = auth;
        _repo = repo;
        _clock = clock;
    }

    public async Task Handle(RemoveStaffOwnerCommand command, CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        staff.RemoveOwner(command.ProfessionalId, _clock);
    }
}