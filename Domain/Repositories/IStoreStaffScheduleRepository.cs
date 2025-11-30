using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreStaffScheduleRepository
{
    Task<StoreStaffScheduleManager> GetByStoreAndProfessionalAsync(int storeId, int professionalId);
    Task SaveAsync(StoreStaffScheduleManager manager);
}