using Domain.Entities;
using Domain.ValueObjects.Identity;

namespace Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId, CancellationToken ct);

    Task<bool> ExistsActiveAsync(Email email, Phone phone, CancellationToken ct);

    Task AddAsync(User user, CancellationToken ct);

    Task<bool> DeleteAsync(int userId, CancellationToken ct);
}
