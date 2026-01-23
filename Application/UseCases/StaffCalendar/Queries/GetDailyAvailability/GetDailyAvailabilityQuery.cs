using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Queries.GetDailyAvailability;

public sealed record GetDailyAvailabilityQuery(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
    ) : IQuery<IReadOnlyCollection<TimeRange>>;