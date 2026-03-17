using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.StoreCalendars;

public sealed class SetStoreCalendarWorkingDayLoader
    : IRequestContextLoader<
        SetStoreCalendarWorkingDayCommand,
        SetStoreCalendarWorkingDayContext>
{
    private readonly IStaffRepository _staffRepo;
    private readonly IStoreCalendarRepository _storeCalendarRepo;

    public SetStoreCalendarWorkingDayLoader(
        IStaffRepository staffRepo,
        IStoreCalendarRepository storeCalendarRepo)
    {
        _staffRepo = staffRepo;
        _storeCalendarRepo = storeCalendarRepo;
    }

    public async Task PopulateAsync(
        SetStoreCalendarWorkingDayCommand command,
        SetStoreCalendarWorkingDayContext ctx,
        CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var calendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        ctx.Staff = staff;
        ctx.StoreCalendar = calendar;
    }
}