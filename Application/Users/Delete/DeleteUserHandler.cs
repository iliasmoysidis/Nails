using Domain.Users.Services;
using Domain.Common;
using MediatR;

namespace Application.Users.Delete;

public sealed class DeleteUserHandler
    : IRequestHandler<DeleteUserCommand>
{
    private readonly DeleteUserContext _ctx;
    private readonly IClock _clock;

    public DeleteUserHandler(DeleteUserContext ctx, IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(DeleteUserCommand command, CancellationToken ct)
    {
        _ctx.UserDeletion.Delete(_clock);

        return Task.CompletedTask;
    }
}
