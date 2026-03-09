using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;

namespace Application.Commands.Stores;

public sealed class UpdateStoreHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateStoreHandler(
        AuthorizationGuard auth,
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateStoreCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var store = await _storeRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        store.UpdateDetails(
            clock: _clock,
            name: ToName(command.Name),
            address: ToAddress(
                command.Street,
                command.City,
                command.PostalCode,
                command.State,
                command.CountryCode
                ),
            phone: ToPhone(command.PhoneCountryCode, command.PhoneNumber)
        );

        await _uow.SaveChangesAsync(ct);
    }

    private static StoreName? ToName(string? name)
        => name is null ? null : StoreName.Create(name);

    private static Address? ToAddress(
        string? street,
        string? city,
        string? postalCode,
        string? state,
        string? countryCode
    )
        => (street is null ||
            city is null ||
            postalCode is null ||
            state is null ||
            countryCode is null)
            ? null
            : Address.From(
                    street: street,
                    city: city,
                    postalCode: postalCode,
                    state: state,
                    countryCode: countryCode
            );

    private static Phone? ToPhone(string? code, string? number)
        => code is null || number is null ? null : Phone.From(code, number);
}