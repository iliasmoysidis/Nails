using Domain.Users;

namespace Application.Users.Update;

public sealed class UpdateUserContext
{
    public User User { get; set; } = default!;
}