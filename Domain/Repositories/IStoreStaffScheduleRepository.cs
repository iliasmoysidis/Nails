using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreStaffScheduleRepository
{
    Task<StoreStaffScheduleManager> GetByStoreIdAndProfessionalIdAsync(int storeId, int professionalId);
    Task SaveAsync(StoreStaffScheduleManager manager);
}