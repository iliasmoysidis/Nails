using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingHandler
{
    private readonly ValidationGuard _val;
    private readonly AuthorizationGuard _auth;
    private readonly IStoreCatalogRepository _storeCatalogRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateOfferingHandler(
        ValidationGuard val,
        AuthorizationGuard auth,
        IStoreCatalogRepository storeCatalogRepo,
        IStaffRepository staffRepo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _val = val;
        _auth = auth;
        _storeCatalogRepo = storeCatalogRepo;
        _staffRepo = staffRepo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateOfferingCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var catalog = await _storeCatalogRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        _val.EnsureStoreOffering(catalog, command.OfferingId);
        _auth.EnsureOwner(staff);

        var offering = catalog.GetOffering(command.OfferingId);

        catalog.UpdateOffering(
            offeringId: command.OfferingId,
            clock: _clock,
            name: ToName(command.Name),
            price: ToPrice(command.Price, offering.Price.Currency),
            duration: ToDuration(command.DurationMinutes),
            description: ToDescription(command.Description)
            );

        await _uow.SaveChangesAsync(ct);
    }

    private static OfferingName? ToName(string? value)
        => value is null ? null : OfferingName.Create(value);

    private static Money? ToPrice(decimal? value, string currency)
        => value is null ? null : Money.Create(value.Value, currency);

    private static Duration? ToDuration(int? minutes)
        => minutes is null ? null : Duration.FromMinutes(minutes.Value);

    private static Description? ToDescription(string? value)
        => value is null ? null : Description.From(value);
}