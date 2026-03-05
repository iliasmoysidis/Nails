using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class AddVacationHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUnitOfWork _uow;

    public AddVacationHandler(
        IManageStaffPolicy policy,
        IStaffCalendarRepository staffCalendarRepo,
        IStaffRepository staffRepo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _staffCalendarRepo = staffCalendarRepo;
        _staffRepo = staffRepo;
        _uow = uow;
    }

    public async Task Handle(AddVacationCommand command, CancellationToken ct)
    {
        var staff = await _staffRepo.GetByStoreId(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _policy.EnsureCanManageStaff(staff);

        var calendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        var holiday = CalendarException.DayOff(command.Date);

        calendar.AddException(holiday);

        await _uow.SaveChangesAsync(ct);
    }
}