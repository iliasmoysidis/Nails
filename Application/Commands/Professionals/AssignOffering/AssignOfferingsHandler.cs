using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;

namespace Application.Commands.Professionals;

public sealed class AssignOfferingsHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IProfessionalOfferingsRepository _repo;
    private readonly IUnitOfWork _uow;

    public AssignOfferingsHandler(
        IManageStorePolicy policy,
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
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var serviceOfferings = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found.");

        foreach (var offeringId in command.OfferingIds)
        {
            serviceOfferings.Assign(command.ProfessionalId, offeringId);
        }

        await _uow.SaveChangesAsync(ct);
    }
}