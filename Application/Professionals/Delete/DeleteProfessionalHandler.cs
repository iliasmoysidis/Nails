using Domain.Professionals.Services;
using Domain.Common;
using MediatR;

namespace Application.Professionals.Delete;

public sealed class DeleteProfessionalHandler
    : IRequestHandler<DeleteProfessionalCommand>
{
    private readonly DeleteProfessionalContext _ctx;
    private readonly IClock _clock;

    public DeleteProfessionalHandler(DeleteProfessionalContext ctx, IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(DeleteProfessionalCommand command, CancellationToken ct)
    {
        _ctx.ProfessionalDeletion.Delete(_clock);

        return Task.CompletedTask;
    }
}
