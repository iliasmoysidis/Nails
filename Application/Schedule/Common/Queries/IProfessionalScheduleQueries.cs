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

    Task<IReadOnlyCollection<StaffCalendarExceptionDTO>> GetExceptionsAsync(
        int storeId,
        int professionalId,
        DateOnly from,
        DateOnly to,
        CancellationToken ct
    );
}