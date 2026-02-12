using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Commands.Stores;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Staffs;

public sealed class RemoveStoreOwnerHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RemoveStoreOwnerHandler(
        IManageStaffPolicy policy,
        IStaffRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RemoveStoreOwnerCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageStaffAsync(command.StoreId, ct);

        var staff = await _repo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        staff.RemoveOwner(command.ProfessionalId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}