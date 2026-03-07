using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Commands.Users;

public sealed class DeleteHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IUserRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public DeleteHandler(
        AuthorizationGuard auth,
        IUserRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _auth = auth;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(DeleteCommand command, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(command.UserId);

        var user = await _repo.GetByIdAsync(command.UserId, ct)
            ?? throw new ApplicationLayerNotFoundException("User not found.");

        user.SoftDelete(_clock);

        await _uow.SaveChangesAsync(ct);
    }
}