using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;
using MediatR;

namespace Application.Features.Stores.Create;

public sealed class Handler
    : IRequestHandler<Command, int>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;

    public Handler(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo
    )
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
    }

    public async Task<int> Handle(
        Command command,
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
            phone: phone
        );

        await _storeRepo.AddAsync(store, ct);

        var staff = Staff.Create(
            storeId: store.Id,
            ownerProfessionalId: command.ProfessionalId
        );

        await _staffRepo.AddAsync(staff, ct);

        return store.Id;
    }
}