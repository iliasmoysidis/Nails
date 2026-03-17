using Domain.Entities;

namespace Application.Commands.Users;

public sealed class DeleteUserContext
{
    public User User { get; set; } = default!;
}