using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.Staffs;

public sealed class RemoveStoreEmployeeHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffRepository _repo;
    private readonly IUnitOfWork _uow;

    public RemoveStoreEmployeeHandler(
        IManageStaffPolicy policy,
        IStaffRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(RemoveStoreEmployeeCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageStaffAsync(command.StoreId, ct);

        var staff = await _repo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        staff.RemoveEmployee(command.ProfessionalId);

        await _uow.SaveChangesAsync(ct);
    }
}