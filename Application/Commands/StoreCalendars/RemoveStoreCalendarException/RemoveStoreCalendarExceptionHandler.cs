using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed class RemoveStoreCalendarExceptionHandler
    : IRequestHandler<RemoveStoreCalendarExceptionCommand>
{
    private readonly RemoveStoreCalendarExceptionContext _ctx;

    public RemoveStoreCalendarExceptionHandler(
        RemoveStoreCalendarExceptionContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        RemoveStoreCalendarExceptionCommand command,
        CancellationToken ct)
    {
        _ctx.StoreCalendar.RemoveException(command.Date);

        return Task.CompletedTask;
    }
}