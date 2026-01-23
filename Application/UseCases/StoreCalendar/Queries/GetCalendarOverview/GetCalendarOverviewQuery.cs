using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StoreCalendar.Queries.GetCalendarOverview;

public sealed record GetCalendarOverviewQuery(int StoreId, DateOnly Date)
    : IQuery<CalendarOverviewDTO>;