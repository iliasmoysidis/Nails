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
    private readonly ICreateOfferingPolicy _policy;
    private readonly IStoreCatalogRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CreateOfferingHandler(
        ICreateOfferingPolicy policy,
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

    public async Task<int> Handle(CreateOfferingCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanCreateAsync(command, ct);

        var catalog = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

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