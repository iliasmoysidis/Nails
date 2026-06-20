using Domain.Roster;
using Domain.Calendar.Services;
using Domain.Stores;
using Application.Abstractions.Context;
using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Features.StoreCalendars.SetDayOff;

public sealed class Loader
    : IRequestContextLoader<
        Command,
        Context>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;

    public Loader(
        IStaffRepository staffRepo,
        IStoreRepository storeRepo,
        IStoreCalendarRepository storeCalendarRepo
    )
    {
        _staffRepo = staffRepo;
        _storeRepo = storeRepo;
        _storeCalendarRepo = storeCalendarRepo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct
    )
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var store = await _storeRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store not found.");

        var calendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        ctx.Staff = staff;
        ctx.StoreAvailability = new StoreAvailability(store, calendar);
    }
}
