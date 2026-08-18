using Application.Common.DTO;
using Application.Schedules.GetExceptions;
using Application.Schedules.GetWeeklySchedule;

namespace Application.Schedules.Common.Queries;

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
