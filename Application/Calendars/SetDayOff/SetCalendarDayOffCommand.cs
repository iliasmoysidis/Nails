using MediatR;

namespace Application.Calendars.SetDayOff;

public sealed record SetCalendarDayOffCommand(
    int StoreId,
    DayOfWeek Day
) : IRequest;