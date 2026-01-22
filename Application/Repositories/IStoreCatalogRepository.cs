using Domain.Entities;

namespace Application.Repositories;

public interface IStoreCatalogRepository
{
    Task<StoreCatalog?> GetAsync(int storeId, CancellationToken ct);
    Task<Staff> GetStaffAsync(int storeId, CancellationToken ct);
}