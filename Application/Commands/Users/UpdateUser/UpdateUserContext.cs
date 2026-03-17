using Domain.Entities;

namespace Application.Commands.Users;

public sealed class UpdateUserContext
{
    public User User { get; set; } = default!;
}