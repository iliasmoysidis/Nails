using MediatR;

namespace Application.Calendars.RemoveException;

public sealed class RemoveCalendarExceptionHandler
    : IRequestHandler<RemoveCalendarExceptionCommand>
{
    private readonly RemoveCalendarExceptionContext _ctx;

    public RemoveCalendarExceptionHandler(RemoveCalendarExceptionContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        RemoveCalendarExceptionCommand command,
        CancellationToken ct)
    {
        _ctx.StoreAvailability.RemoveSpecialOpeningHours(command.Date);

        return Task.CompletedTask;
    }
}
