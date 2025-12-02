using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreStaffRepository
{
    Task<StoreStaff> GetByStoreAsync(int storeId);
    Task SaveAsync(StoreStaff manager);
}