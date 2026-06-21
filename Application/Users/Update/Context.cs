using Domain.Users;

namespace Application.Users.Update;

public sealed class Context
{
    public User User { get; set; } = default!;
}