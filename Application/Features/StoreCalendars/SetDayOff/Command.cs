using MediatR;

namespace Application.Features.StoreCalendars.SetDayOff;

public sealed record Command(
    int StoreId,
    DayOfWeek Day
) : IRequest;