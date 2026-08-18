using Application.Calendars.Common.DTO;
using MediatR;

namespace Application.Calendars.GetCalendar;

public sealed record GetCalendarQuery(
    int StoreId,
    DateOnly From,
    DateOnly To
) : IRequest<IReadOnlyCollection<CalendarDayDTO>>;