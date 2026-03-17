using Application.Abstractions.Repositories;
using Application.Guards;
using Application.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Professionals;

public sealed class DeleteProfessionalHandler
    : IRequestHandler<DeleteProfessionalCommand>
{
    private readonly DeleteProfessionalContext _ctx;
    private readonly IClock _clock;

    public DeleteProfessionalHandler(
        DeleteProfessionalContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(DeleteProfessionalCommand command, CancellationToken ct)
    {
        _ctx.Professional.SoftDelete(_clock);

        return Task.CompletedTask;
    }
}