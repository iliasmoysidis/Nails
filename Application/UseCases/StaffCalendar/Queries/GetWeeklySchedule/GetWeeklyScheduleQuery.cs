using Application.Abstractions;
using Application.DTO;

namespace Application.UseCases.StaffCalendar.Queries.GetWeeklySchedule;

public sealed record GetWeeklyScheduleQuery(
    int StoreId,
    int ProfessionalId,
    DateOnly WeekStart
) : IQuery<IReadOnlyCollection<DailyScheduleDTO>>;