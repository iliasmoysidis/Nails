using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreStaffScheduleRepository
{
    Task<StoreStaffScheduleManager> GetByStoreIdAndProfessionalId(int storeId, int professionalId);
    Task Save(StoreStaffScheduleManager manager);
}