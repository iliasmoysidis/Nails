using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed class SetStoreCalendarWorkingDayHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffRepository _staffRepo;

    public SetStoreCalendarWorkingDayHandler(
        AuthorizationGuard auth,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffRepository staffRepo
    )
    {
        _auth = auth;
        _storeCalendarRepo = storeCalendarRepo;
        _staffRepo = staffRepo;
    }

    public async Task Handle(SetStoreCalendarWorkingDayCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureOwner(staff);

        var calendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        calendar.SetWorkingDay(workingDay);
    }
}