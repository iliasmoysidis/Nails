using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Users;

public sealed class RestoreUserHandler
{
    private readonly IRestoreUserPolicy _policy;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public RestoreUserHandler(
        IRestoreUserPolicy policy,
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(RestoreUserCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanRestoreAsync(command.UserId, ct);

        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        user.Restore(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}