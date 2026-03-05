using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Abstractions.Validation.StaffCalendars;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class AddSpecialAvailabilityHandler
{
    private readonly IScheduleValidator _validator;
    private readonly IManageStaffPolicy _policy;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public AddSpecialAvailabilityHandler(
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

    public async Task Handle(AddSpecialAvailabilityCommand command, CancellationToken ct)
    {
        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var stafCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _validator.EnsureExceptionFitsStoreHours(storeCalendar, exception);

        _policy.EnsureCanManageStaff(staff);

        stafCalendar.AddException(exception);

        await _uow.SaveChangesAsync(ct);
    }
}