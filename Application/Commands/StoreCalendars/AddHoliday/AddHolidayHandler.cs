using Application.Abstractions.Policies.Stores;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StoreCalendars;

public sealed class AddHolidayHandler
{
    private readonly IManageStorePolicy _policy;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public AddHolidayHandler(
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

    public async Task Handle(AddHolidayCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManage(staff);

        var calendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found");

        var holiday = CalendarException.DayOff(command.Date);

        calendar.AddException(holiday);

        await _uow.SaveChangesAsync(ct);
    }
}