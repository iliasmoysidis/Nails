using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.Staffs;

public sealed class RemoveStoreEmployeeHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffRepository _staffRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IUnitOfWork _uow;

    public RemoveStoreEmployeeHandler(
        IManageStaffPolicy policy,
        IStaffRepository staffRepo,
        IProfessionalOfferingsRepository assignmentsRepo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _uow = uow;
    }

    public async Task Handle(RemoveStoreEmployeeCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageStaffAsync(command.StoreId, ct);

        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Professional offerings not found for store {command.StoreId}.");

        assignments.UnassignAllForProfessional(command.ProfessionalId);

        staff.RemoveEmployee(command.ProfessionalId);

        await _uow.SaveChangesAsync(ct);
    }
}