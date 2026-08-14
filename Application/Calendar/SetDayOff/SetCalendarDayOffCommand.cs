using MediatR;

namespace Application.Calendar.SetDayOff;

public sealed record SetCalendarDayOffCommand(
    int StoreId,
    DayOfWeek Day
) : IRequest;