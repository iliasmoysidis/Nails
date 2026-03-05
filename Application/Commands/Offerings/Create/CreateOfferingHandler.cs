using Application.Abstractions.Policies.Offerings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Application.Commands.Offerings;

public sealed class CreateOfferingHandler
{
    private readonly IManageOfferingPolicy _policy;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CreateOfferingHandler(
        IManageOfferingPolicy policy,
        IStoreCatalogRepository storeCatalogRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _storeCatalogRepo = storeCatalogRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(CreateOfferingCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManage(staff);

        var catalog = await _storeCatalogRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store catalog not found for store.");

        var offering = catalog.AddOffering(
            name: OfferingName.Create(command.Name),
            price: Money.Create(amount: command.Price, currency: command.Currency),
            duration: Duration.FromMinutes(command.DurationMinutes),
            description: Description.From(command.Description),
            clock: _clock
        );

        await _uow.SaveChangesAsync(ct);

        return offering.Id;
    }
}