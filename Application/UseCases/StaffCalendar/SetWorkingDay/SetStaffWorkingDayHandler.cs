using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Services;

namespace Application.UseCases.StaffCalendar.SetWorkingDay;

public sealed class SetStaffWorkingDayHandler : ICommandHandler<SetStaffWorkingDayCommand>
{
    private readonly IStaffCalendarRepository _repo;
    private readonly StaffCalendarService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public SetStaffWorkingDayHandler(
        IStaffCalendarRepository repo,
        StaffCalendarService service,
        ICurrentUser currentUser,
        IUnitOfWork uow
    )
    {
        _repo = repo;
        _service = service;
        _currentUser = currentUser;
        _uow = uow;
    }

    public async Task Handle(SetStaffWorkingDayCommand command, CancellationToken ct)
    {
        var calendar = await _repo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found.");

        var staff = await _repo.GetStaffAsync(command.StoreId, ct);

        var otherCalendars = await _repo.GetOtherCalendarsAsync(command.StoreId, command.ProfessionalId, ct);

        _service.SetWorkingDay(calendar, otherCalendars, staff, _currentUser.UserId, command.WorkingDay);

        await _uow.SaveChangesAsync(ct);
    }
}