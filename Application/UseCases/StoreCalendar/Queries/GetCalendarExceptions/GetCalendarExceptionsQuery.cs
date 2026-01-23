using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.Queries.GetCalendarExceptions;

public sealed record GetCalendarExceptionsQuery(int StoreId)
    : IQuery<IReadOnlyCollection<CalendarException>>;