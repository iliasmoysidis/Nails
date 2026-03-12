using Application.DTO.Calendar;

namespace Application.Abstractions.Queries;

public interface IStoreCalendarQueries
{
    Task<IReadOnlyCollection<CalendarDayDTO>> GetStoreCalendarAsync(
        int storeId,
        DateOnly from,
        DateOnly to,
        CancellationToken ct
    );

    Task<IReadOnlyCollection<CalendarDayDTO>> GetProfessionalCalendarAsync(
        int storeId,
        int professionalId,
        DateOnly from,
        DateOnly to,
        CancellationToken ct
    );

    Task<IReadOnlyCollection<AvailableSlotDTO>> GetAvailableSlotsAsync(
        int storeId,
        int professionalId,
        int offeringId,
        DateOnly date,
        CancellationToken ct
    );
}