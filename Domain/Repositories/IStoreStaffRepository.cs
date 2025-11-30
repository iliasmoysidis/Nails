using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreStaffRepository
{
    Task<StoreStaffManager> GetByStoreAsync(int storeId);
    Task SaveAsync(StoreStaffManager manager);
}