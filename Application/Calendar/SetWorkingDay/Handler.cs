using Domain.Common.ValueObjects.Calendar;
using MediatR;

namespace Application.Calendar.SetWorkingDay;

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

        _ctx.StoreAvailability.SetOpeningHours(command.Day, ranges);

        return Task.CompletedTask;
    }
}
