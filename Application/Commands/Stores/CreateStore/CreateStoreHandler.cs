using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;
using MediatR;

namespace Application.Commands.Stores;

public sealed class CreateStoreHandler
    : IRequestHandler<CreateStoreCommand, int>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;

    public CreateStoreHandler(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IClock clock)
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _clock = clock;
    }

    public async Task<int> Handle(
        CreateStoreCommand command,
        CancellationToken ct)
    {
        var address = Address.From(
            command.Street,
            command.City,
            command.PostalCode,
            command.State,
            command.CountryCode
        );

        var taxId = TaxIdentificationNumber.From(
            command.TaxCountryCode,
            command.TaxNumber
        );

        var phone = Phone.From(
            command.PhoneCountryCode,
            command.PhoneNumber
        );

        var store = Store.Create(
            name: StoreName.Create(command.Name),
            address: address,
            taxIdNumber: taxId,
            email: Email.From(command.Email),
            phone: phone,
            clock: _clock
        );

        await _storeRepo.AddAsync(store, ct);

        var staff = Staff.Create(
            storeId: store.Id,
            professionalId: command.ProfessionalId,
            clock: _clock
        );

        await _staffRepo.AddAsync(staff, ct);

        return store.Id;
    }
}