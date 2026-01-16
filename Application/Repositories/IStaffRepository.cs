using Domain.Entities;

namespace Domain.Repositories;

public interface IStaffRepository
{
    Task<Staff?> GetByStoreAsync(int storeId);
    void Add(Staff staff);
}