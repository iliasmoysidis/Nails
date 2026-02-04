using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Users;

public sealed class DeleteUserHandler
{
    private readonly IDeleteUserPolicy _policy;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public DeleteUserHandler(
        IDeleteUserPolicy policy,
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

    public async Task Handle(DeleteUserCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanDeleteAsync(command.UserId, ct);

        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        user.SoftDelete(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}