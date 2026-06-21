using Domain.Users.Services;

namespace Application.Users.Delete;

public sealed class Context
{
    public UserDeletion UserDeletion { get; set; } = null!;
}
