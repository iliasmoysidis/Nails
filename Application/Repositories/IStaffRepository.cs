using Domain.Entities;

namespace Application.Repositories;

public interface IStaffRepository
{
    Task<Staff> GetStaffAsync(int storeId, CancellationToken ct);
}