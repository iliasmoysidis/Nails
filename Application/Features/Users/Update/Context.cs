using Domain.Users;

namespace Application.Features.Users.Update;

public sealed class Context
{
    public User User { get; set; } = default!;
}