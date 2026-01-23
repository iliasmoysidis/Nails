using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Queries.GetCalendarExceptions;

public sealed record GetCalendarExceptionsQuery(int StoreId, int ProfessionalId)
    : IQuery<IReadOnlyCollection<CalendarException>>;