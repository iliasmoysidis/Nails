using Application.DTO.User;

namespace Application.Abstractions.Queries;

public interface IUserQueries
{
    Task<UserProfileDTO?> GetUserProfileAsync(int userId, CancellationToken ct);
}