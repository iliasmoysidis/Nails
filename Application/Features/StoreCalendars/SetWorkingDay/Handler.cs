using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Features.StoreCalendars.SetWorkingDay;

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

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        _ctx.StoreCalendar.SetWorkingDay(workingDay);

        return Task.CompletedTask;
    }
}