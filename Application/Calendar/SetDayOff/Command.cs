using MediatR;

namespace Application.Calendar.SetDayOff;

public sealed record Command(
    int StoreId,
    DayOfWeek Day
) : IRequest;