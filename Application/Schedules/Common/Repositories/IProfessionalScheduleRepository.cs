using Domain.Schedules.Entities;
using Domain.Schedules;

namespace Application.Schedules.Common.Repositories;

public interface IProfessionalScheduleRepository
{
    Task<ProfessionalSchedule?> GetByProfessionalIdAsync(int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<ProfessionalSchedule>> GetByStoreIdAsync(int storeId, CancellationToken ct);
}
