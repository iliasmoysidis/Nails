using Application.Common.DTO;
using Application.Schedule.GetExceptions;
using Application.Schedule.GetWeeklySchedule;

namespace Application.Schedule.Common.Queries;

public interface IProfessionalScheduleQueries
{
    Task<IReadOnlyCollection<StaffWorkingDayDTO>> GetWeeklyScheduleAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
    );

    Task<PagedResult<StaffCalendarExceptionDTO>> GetExceptionsAsync(
        int storeId,
        int professionalId,
        DateOnly from,
        DateOnly to,
        int? page,
        int? limit,
        CancellationToken ct
    );
}
