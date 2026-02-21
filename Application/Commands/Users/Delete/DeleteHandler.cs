using Application.Abstractions.Policies.Users;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Users;

public sealed class DeleteHandler
{
    private readonly IManageUserPolicy _policy;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public DeleteHandler(
        IManageUserPolicy policy,
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

    public async Task Handle(DeleteCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.UserId, ct);

        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        user.SoftDelete(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}