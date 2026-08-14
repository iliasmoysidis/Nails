using Domain.Common.ValueObjects.Calendar;
using MediatR;

namespace Application.Schedule.AddSpecialAvailability;

public sealed class AddSpecialAvailabilityHandler
    : IRequestHandler<AddSpecialAvailabilityCommand>
{
    private readonly AddSpecialAvailabilityContext _ctx;

    public AddSpecialAvailabilityHandler(AddSpecialAvailabilityContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddSpecialAvailabilityCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _ctx.ProfessionalAvailability.SetException(exception);

        return Task.CompletedTask;
    }
}
