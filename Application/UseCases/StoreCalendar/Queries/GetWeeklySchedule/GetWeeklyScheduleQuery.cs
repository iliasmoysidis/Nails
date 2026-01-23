using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StoreCalendar.Queries.GetWeeklySchedule;

public sealed record GetWeeklyScheduleQuery(
    int StoreId,
    DateOnly WeekStart
    )
        : IQuery<IReadOnlyCollection<DailyScheduleDTO>>;