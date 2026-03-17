using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarSpecialAvailabilityHandler
{
    private readonly ValidationGuard _val;
    private readonly AuthorizationGuard _auth;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStaffRepository _staffRepo;

    public AddStaffCalendarSpecialAvailabilityHandler(
        ValidationGuard val,
        AuthorizationGuard auth,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IStaffRepository staffRepo
    )
    {
        _val = val;
        _auth = auth;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _staffRepo = staffRepo;
    }

    public async Task Handle(AddStaffCalendarSpecialAvailabilityCommand command, CancellationToken ct)
    {
        var storeCalendar = await _storeCalendarRepo.GetByIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _val.EnsureExceptionFitsStoreHours(storeCalendar, exception);

        _auth.EnsureOwner(staff);

        staffCalendar.AddException(exception);
    }
}