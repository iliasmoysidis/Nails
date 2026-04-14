using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.ValueObjects.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _db.Users.AddAsync(user, ct);
    }

    public async Task<bool> DeleteAsync(int userId, CancellationToken ct)
    {
        var affected = await _db.Users
            .Where(x => x.Id == userId)
            .ExecuteDeleteAsync(ct);

        return affected > 0;
    }

    public async Task<bool> ExistsAsync(Email email, Phone phone, CancellationToken ct)
    {
        return await _db.Users
            .Where(x => x.Email.Value == email.Value ||
                (x.Phone.CountryCode == phone.CountryCode &&
                x.Phone.NationalNumber == phone.NationalNumber
                )
            ).AnyAsync(ct);
    }

    public async Task<User?> GetByIdAsync(int userId, CancellationToken ct)
    {
        return await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, ct);
    }
}
