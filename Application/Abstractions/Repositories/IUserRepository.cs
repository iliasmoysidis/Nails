using Domain.Entities;
using Domain.ValueObjects.Identity;

namespace Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken ct);

    Task<User?> GetByPhoneAsync(Phone phone, CancellationToken ct);

    Task AddAsync(User user, CancellationToken ct);
}