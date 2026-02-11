using Application.Abstractions.Policies.Offerings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingHandler
{
    private readonly IManageOfferingPolicy _policy;
    private readonly IStoreCatalogRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateOfferingHandler(
        IManageOfferingPolicy policy,
        IStoreCatalogRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateOfferingCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var catalog = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        var offering = catalog.GetOfferingOrThrow(command.OfferingId);

        OfferingName? name = command.Name switch
        {
            null => null,
            _ => OfferingName.Create(command.Name)
        };

        Money? price = command.Price switch
        {
            null => null,
            _ => Money.Create(command.Price.Value, offering.Price.Currency)
        };

        Duration? duration = command.DurationMinutes switch
        {
            null => null,
            _ => Duration.FromMinutes(command.DurationMinutes.Value)
        };

        Description? description = command.Description switch
        {
            null => null,
            _ => Description.From(command.Description)
        };

        catalog.UpdateOffering(
            offeringId: command.OfferingId,
            clock: _clock,
            name: name,
            price: price,
            duration: duration,
            description: description
            );

        await _uow.SaveChangesAsync(ct);
    }
}