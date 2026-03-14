using Application.DTO.StaffCalendar;

namespace Application.Abstractions.Queries;

public interface IStaffCalendarQueries
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