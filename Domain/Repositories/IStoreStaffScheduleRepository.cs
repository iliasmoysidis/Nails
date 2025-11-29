using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreStaffScheduleRepository
{
    StoreStaffScheduleManager GetByStoreIdAndProfessionalId(int storeId, int professionalId);
    void Save(StoreStaffScheduleManager manager);
}