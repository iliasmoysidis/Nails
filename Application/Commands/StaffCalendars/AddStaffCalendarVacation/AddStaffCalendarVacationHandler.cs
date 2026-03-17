using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarVacationHandler
    : IRequestHandler<AddStaffCalendarVacationCommand>
{
    private readonly AddStaffCalendarVacationContext _ctx;

    public AddStaffCalendarVacationHandler(
        AddStaffCalendarVacationContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddStaffCalendarVacationCommand command,
        CancellationToken ct)
    {
        var holiday = CalendarException.DayOff(command.Date);

        _ctx.StaffCalendar.AddException(holiday);

        return Task.CompletedTask;
    }
}