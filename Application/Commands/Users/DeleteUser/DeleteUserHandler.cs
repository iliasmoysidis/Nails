using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Users;

public sealed class DeleteUserHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;


    public DeleteUserHandler(
        AuthorizationGuard auth,
        IUserRepository repo,
        IClock clock
    )
    {
        _auth = auth;
        _repo = repo;
        _clock = clock;
    }

    public async Task Handle(DeleteUserCommand command, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(command.UserId);

        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        user.SoftDelete(_clock);
    }
}