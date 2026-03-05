using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.ProfessionalOfferings;

public sealed class UnassignHandler
{
    private readonly IUnassignOfferingPolicy _policy;
    private readonly IProfessionalOfferingsRepository _professionalRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public UnassignHandler(
        IUnassignOfferingPolicy policy,
        IProfessionalOfferingsRepository professionalRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _professionalRepo = professionalRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(UnassignCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanUnassignOffering(
            command.ProfessionalId,
            staff
        );

        var professionalOfferings = await _professionalRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            professionalOfferings.Unassign(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}