using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Users;

public sealed class DeleteUserLoader
    : IRequestContextLoader<DeleteUserCommand, DeleteUserContext>
{
    private readonly IUserRepository _repo;

    public DeleteUserLoader(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        DeleteUserCommand command,
        DeleteUserContext ctx,
        CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        ctx.User = user;
    }
}