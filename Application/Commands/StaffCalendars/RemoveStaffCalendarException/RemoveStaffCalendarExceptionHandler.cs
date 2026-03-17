using MediatR;

namespace Application.Commands.StaffCalendars;

public sealed class RemoveStaffCalendarExceptionHandler
    : IRequestHandler<RemoveStaffCalendarExceptionCommand>
{
    private readonly RemoveStaffCalendarExceptionContext _ctx;

    public RemoveStaffCalendarExceptionHandler(
        RemoveStaffCalendarExceptionContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        RemoveStaffCalendarExceptionCommand command,
        CancellationToken ct)
    {
        _ctx.StaffCalendar.RemoveException(command.Date);

        return Task.CompletedTask;
    }
}