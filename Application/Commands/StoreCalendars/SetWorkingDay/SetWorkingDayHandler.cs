using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed class SetWorkingDayHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public SetWorkingDayHandler(
        IManageStorePolicy policy,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _storeCalendarRepo = storeCalendarRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(SetWorkingDayCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManage(staff);

        var calendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        calendar.SetWorkingDay(workingDay);

        await _uow.SaveChangesAsync(ct);
    }
}