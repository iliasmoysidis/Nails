using Application.Users.Common.Repositories;
using Infrastructure.Common;
using Domain.Users;
using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
    }

    public async Task<bool> ExistsAsync(Email email, Phone phone, CancellationToken ct)
    {
        return await _context.Users
            .Where(x => x.Email == email || x.Phone == phone)
            .AnyAsync(ct);
    }

    public async Task<User?> GetByIdAsync(int userId, CancellationToken ct)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, ct);
    }
}
