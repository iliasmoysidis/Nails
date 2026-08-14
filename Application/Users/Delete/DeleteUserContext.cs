using Domain.Users.Services;

namespace Application.Users.Delete;

public sealed class DeleteUserContext
{
    public UserDeletion UserDeletion { get; set; } = null!;
}
