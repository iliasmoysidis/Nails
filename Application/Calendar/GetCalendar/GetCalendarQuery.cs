using Application.Calendar.Common.DTO;
using MediatR;

namespace Application.Calendar.GetCalendar;

public sealed record GetCalendarQuery(
    int StoreId,
    DateOnly From,
    DateOnly To
) : IRequest<IReadOnlyCollection<CalendarDayDTO>>;