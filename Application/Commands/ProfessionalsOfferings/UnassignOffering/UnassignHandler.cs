using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;

namespace Application.Commands.ProfessionalOfferings;

public sealed class UnassignHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IProfessionalOfferingsRepository _professionalRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public UnassignHandler(
        AuthorizationGuard auth,
        IProfessionalOfferingsRepository professionalRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _professionalRepo = professionalRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(UnassignCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var professionalOfferings = await _professionalRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            professionalOfferings.Unassign(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}