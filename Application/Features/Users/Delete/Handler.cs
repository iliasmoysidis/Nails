using Application.Abstractions.Repositories;
using Application.Exceptions;
using MediatR;

namespace Application.Features.Users.Delete;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly IUserRepository _repo;

    public Handler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(Command command, CancellationToken ct)
    {
        var isDeleted = await _repo.DeleteAsync(command.UserId, ct);

        if (!isDeleted)
            throw new ApplicationLayerNotFoundException("User not found.");
    }
}