using Application.Abstractions.Policies.Offerings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Offerings;

public sealed class RemoveOfferingHandler
{
    private readonly IManageOfferingPolicy _policy;
    private readonly IStoreCatalogRepository _catalogRepo;
    private readonly IProfessionalOfferingsRepository _assignmentsRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RemoveOfferingHandler(
        IManageOfferingPolicy policy,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStoreCatalogRepository catalogRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RemoveOfferingCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var catalog = await _catalogRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Professional offerings not found for store {command.StoreId}.");

        assignments.UnassignAllForOffering(command.OfferingId);

        catalog.RemoveOffering(command.OfferingId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}