using Domain.Schedule.Entities;
using Domain.Schedule;

namespace Application.Schedule.Common.Repositories;

public interface IProfessionalScheduleRepository
{
    Task<ProfessionalSchedule?> GetByProfessionalIdAsync(int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<ProfessionalSchedule>> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task AddCalendarAsync(StaffCalendar calendar, CancellationToken ct);

    Task RemoveCalendarAsync(int professionalId, int storeId, CancellationToken ct);
}
