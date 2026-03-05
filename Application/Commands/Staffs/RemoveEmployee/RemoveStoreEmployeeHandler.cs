using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Staffs;

public sealed class RemoveStoreEmployeeHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffRepository _staffRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RemoveStoreEmployeeHandler(
        IManageStaffPolicy policy,
        IStaffRepository staffRepo,
        IProfessionalOfferingsRepository assignmentsRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _staffRepo = staffRepo;
        _assignmentsRepo = assignmentsRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RemoveStoreEmployeeCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManageStaff(staff);

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Professional offerings not found for store {command.StoreId}.");

        assignments.UnassignAllForProfessional(command.ProfessionalId);

        staff.RemoveEmployee(command.ProfessionalId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}