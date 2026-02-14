using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.ProfessionalOfferings;

public sealed class AssignOfferingsHandler
{
    private readonly IAssignOfferingPolicy _policy;
    private readonly IProfessionalOfferingsRepository _repo;
    private readonly IUnitOfWork _uow;

    public AssignOfferingsHandler(
        IAssignOfferingPolicy policy,
        IProfessionalOfferingsRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(AssignOfferingsCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanAssignOfferingAsync(command.StoreId, ct);

        var professionalOfferings = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Professional offerings not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            professionalOfferings.Assign(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}