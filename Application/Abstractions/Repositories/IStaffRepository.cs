using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStaffRepository
{
    Task<Staff?> GetByStoreId(int storeId, CancellationToken ct);
}