using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.StaffCalendars;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class SetWorkingDayHandler
{
    private readonly IScheduleValidator _validator;
    private readonly IManageStaffPolicy _policy;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public SetWorkingDayHandler(
        IScheduleValidator validator,
        IManageStaffPolicy policy,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _validator = validator;
        _policy = policy;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(SetWorkingDayCommand command, CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        _validator.EnsureWorkingDayFitsStoreHours(storeCalendar, workingDay);

        _policy.EnsureCanManageStaff(staff);

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        staffCalendar.SetWorkingDay(workingDay);

        await _uow.SaveChangesAsync(ct);
    }
}