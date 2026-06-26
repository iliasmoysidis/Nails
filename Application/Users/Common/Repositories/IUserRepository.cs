using Domain.Users;
using Domain.Common.ValueObjects;

namespace Application.Users.Common.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId, CancellationToken ct);

    Task<bool> ExistsAsync(Email email, Phone phone, CancellationToken ct);

    Task AddAsync(User user, CancellationToken ct);
}
