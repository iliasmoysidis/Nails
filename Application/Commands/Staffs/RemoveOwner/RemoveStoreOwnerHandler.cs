using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Commands.Stores;
using Application.Exceptions;

namespace Application.Commands.Staffs;

public sealed class RemoveStoreOwnerHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffRepository _repo;
    private readonly IUnitOfWork _uow;

    public RemoveStoreOwnerHandler(
        IManageStaffPolicy policy,
        IStaffRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(RemoveStoreOwnerCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageStaffAsync(command.StoreId, ct);

        var staff = await _repo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        staff.RemoveOwner(command.ProfessionalId);

        await _uow.SaveChangesAsync(ct);
    }
}