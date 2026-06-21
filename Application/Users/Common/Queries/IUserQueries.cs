using Application.Users.GetProfile;

namespace Application.Users.Common.Queries;

public interface IUserQueries
{
    Task<UserProfileDTO?> GetUserProfileAsync(int userId, CancellationToken ct);
}