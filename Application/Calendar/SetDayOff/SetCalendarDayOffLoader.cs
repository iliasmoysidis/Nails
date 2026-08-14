using Application.Calendar.Common.Repositories;
using Application.Roster.Common.Repositories;
using Application.Stores.Common.Repositories;
using Domain.Roster;
using Domain.Calendar.Services;
using Domain.Stores;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Calendar.SetDayOff;

public sealed class SetCalendarDayOffLoader
    : IRequestContextLoader<
        SetCalendarDayOffCommand,
        SetCalendarDayOffContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreRepository _storeRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;

    public SetCalendarDayOffLoader(
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
        SetCalendarDayOffCommand command,
        SetCalendarDayOffContext ctx,
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
