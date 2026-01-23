using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.Services;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Commands.AddException;

public sealed class AddExceptionHandler : ICommandHandler<AddExceptionCommand>
{
    private readonly IStaffCalendarRepository _repo;
    private readonly StaffCalendarService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _uow;

    public AddExceptionHandler(
        IStaffCalendarRepository repo,
        StaffCalendarService service,
        ICurrentUser currentUser,
        IUnitOfWork uow)
    {
        _repo = repo;
        _service = service;
        _currentUser = currentUser;
        _uow = uow;
    }

    public async Task Handle(AddExceptionCommand command, CancellationToken ct)
    {
        var calendar = await _repo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found.");

        var staff = await _repo.GetStaffAsync(command.StoreId, ct);

        var otherCalendars = await _repo.GetOtherCalendarsAsync(command.StoreId, command.ProfessionalId, ct);

        var exception = command.IsDayOff
        ? CalendarException.DayOff(command.Date)
        : CalendarException.PartialDay(command.Date, command.TimeRanges);

        _service.AddException(calendar, otherCalendars, staff, _currentUser.UserId, exception);

        await _uow.SaveChangesAsync(ct);
    }
}