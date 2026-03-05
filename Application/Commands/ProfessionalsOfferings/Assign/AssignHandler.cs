using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.ProfessionalOfferings;

public sealed class AssignHandler
{
    private readonly IAssignOfferingPolicy _policy;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public AssignHandler(
        IAssignOfferingPolicy policy,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _assignmentsRepo = assignmentsRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(AssignCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanAssignOffering(staff);

        var professionalOfferings = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            professionalOfferings.Assign(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}