using Application.Abstractions.Policies.Staffs;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class AddVacationHandler
{
    private readonly IManageStaffPolicy _policy;
    private readonly IStaffCalendarRepository _repo;
    private readonly IUnitOfWork _uow;

    public AddVacationHandler(
        IManageStaffPolicy policy,
        IStaffCalendarRepository repo,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(AddVacationCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageStaffAsync(command.StoreId, ct);

        var calendar = await _repo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff calendar not found.");

        var holiday = CalendarException.DayOff(command.Date);

        calendar.AddException(holiday);

        await _uow.SaveChangesAsync(ct);
    }
}