using Domain.Entities;

namespace Domain.Repositories;

public interface IStaffRepository
{
    Task<Staff> GetByStoreAsync(int storeId);
    Task SaveAsync(Staff manager);
}