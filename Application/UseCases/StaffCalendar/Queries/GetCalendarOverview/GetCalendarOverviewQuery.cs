using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StaffCalendar.Queries.GetCalendarOverview;

public sealed record GetCalendarOverviewQuery(int StoreId, int ProfessionalId, DateOnly Date)
    : IQuery<CalendarOverviewDTO>;