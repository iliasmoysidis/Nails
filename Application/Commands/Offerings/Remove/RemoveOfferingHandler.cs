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
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RemoveOfferingHandler(
        IManageOfferingPolicy policy,
        IProfessionalOfferingsRepository assignmentsRepo,
        IStoreCatalogRepository catalogRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _catalogRepo = catalogRepo;
        _assignmentsRepo = assignmentsRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RemoveOfferingCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not foud.");

        _policy.EnsureCanManage(staff);

        var catalog = await _catalogRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        var assignments = await _assignmentsRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Professional offerings not found for store {command.StoreId}.");

        assignments.UnassignAllForOffering(command.OfferingId);

        catalog.RemoveOffering(command.OfferingId, _clock);

        await _uow.SaveChangesAsync(ct);
    }
}