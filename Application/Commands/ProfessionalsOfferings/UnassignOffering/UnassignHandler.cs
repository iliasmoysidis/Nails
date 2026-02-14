using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.ProfessionalOfferings;

public sealed class UnassignHandler
{
    private readonly IUnassignOfferingPolicy _policy;
    private readonly IProfessionalOfferingsRepository _repo;
    private readonly IUnitOfWork _uow;

    public UnassignHandler(
        IUnassignOfferingPolicy policy,
        IProfessionalOfferingsRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(UnassignCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanUnassignOfferingAsync(
            storeId: command.StoreId,
            professionalId: command.ProfessionalId,
            ct: ct
        );

        var professionalOfferings = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            professionalOfferings.Unassign(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}