using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Store;

namespace Application.Commands.Stores;

public sealed class UpdateDetailsHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreRepository _storeRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateDetailsHandler(
        IManageStorePolicy policy,
        IStoreRepository storeRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _storeRepo = storeRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateDetailsCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManage(staff);

        var store = await _storeRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        store.UpdateDetails(
            clock: _clock,
            name: command.Name is null
                ? null
                : StoreName.Create(command.Name),
            address: command.Street is null
                ? null
                : Address.From(
                    street: command.Street,
                    city: command.City!,
                    postalCode: command.PostalCode!,
                    state: command.State!,
                    countryCode: command.CountryCode!
                ),
            phone: command.PhoneCountryCode is null
                ? null
                : Phone.From(
                    countryCode: command.PhoneCountryCode,
                    nationalNumber: command.PhoneNumber!
                )
        );

        await _uow.SaveChangesAsync(ct);
    }
}