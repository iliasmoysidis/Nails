using Application.Users.Common.Repositories;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Users.Update;

public sealed class Loader
    : IRequestContextLoader<Command, Context>
{
    private readonly IUserRepository _repo;

    public Loader(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task PopulateAsync(
        Command command,
        Context ctx,
        CancellationToken ct
    )
    {
        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        ctx.User = user;
    }
}