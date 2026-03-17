using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Users;

public sealed class DeleteUserHandler
    : IRequestHandler<DeleteUserCommand>
{
    private readonly DeleteUserContext _ctx;
    private readonly IClock _clock;

    public DeleteUserHandler(
        DeleteUserContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(DeleteUserCommand command, CancellationToken ct)
    {
        _ctx.User.SoftDelete(_clock);

        return Task.CompletedTask;
    }
}