using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;

namespace Application.Commands.Stores;

public sealed class CreateStoreHandler
{
    private readonly IStoreRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CreateStoreHandler(
        IStoreRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task<int> Handle(CreateStoreCommand command, CancellationToken ct)
    {
        var store = Store.Create(
            name: StoreName.Create(command.Name),
            address: Address.From(
                street: command.Street,
                city: command.City,
                postalCode: command.PostalCode,
                state: command.State,
                countryCode: command.CountryCode
            ),
            taxIdNumber: TaxIdentificationNumber.From(
                countryCode: command.TaxCountryCode,
                value: command.TaxNumber
            ),
            email: Email.From(command.Email),
            phone: Phone.From(
                countryCode: command.CountryCode,
                nationalNumber: command.PhoneNumber
            ),
            clock: _clock
        );

        await _repo.AddAsync(store, ct);
        await _uow.SaveChangesAsync(ct);

        return store.Id;
    }
}