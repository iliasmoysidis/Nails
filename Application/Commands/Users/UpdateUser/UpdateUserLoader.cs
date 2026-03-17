using Application.Abstractions.Repositories;
using Application.Exceptions;

namespace Application.Commands.Users;

public sealed class UpdateUserLoader
    : IRequestContextLoader<UpdateUserCommand, UpdateUserContext>
{
    private readonly IUserRepository _repo;

    public UpdateUserLoader(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        UpdateUserCommand command,
        UpdateUserContext ctx,
        CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        ctx.User = user;
    }
}