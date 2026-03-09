using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;

namespace Application.Commands.Stores;

public sealed class CreateStoreHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public CreateStoreHandler(
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

    public async Task<int> Handle(CreateStoreCommand command, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(command.ProfessionalId);

        var address = Address.From(
                street: command.Street,
                city: command.City,
                postalCode: command.PostalCode,
                state: command.State,
                countryCode: command.CountryCode
            );
        var taxIdNumber = TaxIdentificationNumber.From(
                countryCode: command.TaxCountryCode,
                value: command.TaxNumber
            );
        var phone = Phone.From(
                countryCode: command.PhoneCountryCode,
                nationalNumber: command.PhoneNumber
            );

        var store = Store.Create(
            name: StoreName.Create(command.Name),
            address: address,
            taxIdNumber: taxIdNumber,
            email: Email.From(command.Email),
            phone: phone,
            clock: _clock
        );
        await _storeRepo.AddAsync(store, ct);

        await _uow.SaveChangesAsync(ct);

        var staff = Staff.Create(
            storeId: store.Id,
            professionalId: command.ProfessionalId,
            clock: _clock
        );
        await _staffRepo.AddAsync(staff, ct);

        await _uow.SaveChangesAsync(ct);

        return store.Id;
    }
}