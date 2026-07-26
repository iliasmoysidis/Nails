using Application.Users.Common.Queries;
using Application.Users.GetProfile;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users;

public sealed class UserQueries : IUserQueries
{
    private readonly AppDbContext _context;

    public UserQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDTO?> GetUserProfileAsync(int userId, CancellationToken ct)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(
                u => new UserProfileDTO(
                    u.Id,
                    u.FullName.ToString(),
                    u.Email.ToString(),
                    u.Phone.ToString()
                )
            )
            .FirstOrDefaultAsync(ct);
    }
}
