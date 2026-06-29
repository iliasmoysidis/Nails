using Application.Assignments.Common.Repositories;
using Application.Calendar.Common.Repositories;
using Application.Catalog.Common.Repositories;
using Application.Roster.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Catalog;
using Domain.Roster;
using Domain.Calendar;
using Domain.Stores.Services;
using Domain.Stores;
using Domain.Common.ValueObjects;
using Domain.Stores.ValueObjects;
using MediatR;

namespace Application.Stores.Create;

public sealed class Handler
    : IRequestHandler<Command, int>
{
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IAssignmentRegistryRepository _assignmentRegistryRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;

    public Handler(
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IStoreCatalogRepository storeCatalogRepo,
        IAssignmentRegistryRepository assignmentRegistryRepo,
        IStoreCalendarRepository storeCalendarRepo
    )
    {
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _storeCatalogRepo = storeCatalogRepo;
        _assignmentRegistryRepo = assignmentRegistryRepo;
        _storeCalendarRepo = storeCalendarRepo;
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

        var store = new Store(
            name: StoreName.Create(command.Name),
            address: address,
            taxIdNumber: taxId,
            email: Email.From(command.Email),
            phone: phone
        );

        await _storeRepo.AddAsync(store, ct);

        var storeSetup = new StoreSetup(store.Id, command.ProfessionalId);

        await _staffRepo.AddAsync(storeSetup.Staff, ct);
        await _storeCatalogRepo.AddAsync(storeSetup.StoreCatalog, ct);
        await _assignmentRegistryRepo.AddAsync(storeSetup.Assignments, ct);
        await _storeCalendarRepo.AddAsync(storeSetup.StoreCalendar, ct);

        return store.Id;
    }
}
