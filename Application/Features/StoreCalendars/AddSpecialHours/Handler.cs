using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Features.StoreCalendars.AddSpecialHours;

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
        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        _ctx.StoreCalendar.SetSpecialOpeningHours(command.Date, ranges);

        return Task.CompletedTask;
    }
}
