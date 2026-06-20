using Domain.Common.ValueObjects.Calendar;
using MediatR;

namespace Application.Features.StaffCalendars.AddSpecialAvailability;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        Command command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _ctx.ProfessionalAvailability.SetException(exception);

        return Task.CompletedTask;
    }
}
