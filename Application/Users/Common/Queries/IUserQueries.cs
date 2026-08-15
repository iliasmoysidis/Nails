using Application.Users.GetDetails;

namespace Application.Users.Common.Queries;

public interface IUserQueries
{
    Task<UserDTO?> GetUserDetailsAsync(int userId, CancellationToken ct);
}
