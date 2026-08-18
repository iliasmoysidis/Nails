using Domain.Common.ValueObjects.Calendars;
using MediatR;

namespace Application.Calendars.AddSpecialHours;

public sealed class AddSpecialHoursHandler
    : IRequestHandler<AddSpecialHoursCommand>
{
    private readonly AddSpecialHoursContext _ctx;

    public AddSpecialHoursHandler(AddSpecialHoursContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddSpecialHoursCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        _ctx.StoreAvailability.SetSpecialOpeningHours(command.Date, ranges);

        return Task.CompletedTask;
    }
}
