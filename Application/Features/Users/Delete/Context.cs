using Domain.Users.Services;

namespace Application.Features.Users.Delete;

public sealed class Context
{
    public UserDeletion UserDeletion { get; set; } = null!;
}
