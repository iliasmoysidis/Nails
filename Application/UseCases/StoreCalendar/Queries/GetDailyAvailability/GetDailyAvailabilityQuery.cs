using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.Queries.GetDailyAvailability;

public sealed record GetDailyAvailabilityQuery(int StoreId, DateOnly Date)
    : IQuery<IReadOnlyCollection<TimeRange>>;