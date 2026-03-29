using Domain.Interfaces;
using MediatR;

namespace Application.Features.Professionals.LeaveStore;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;
    private readonly IClock _clock;

    public Handler(
        Context ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public async Task Handle(
        Command command,
        CancellationToken ct)
    {
        _ctx.Staff.RemoveProfessional(
                professionalId: command.ProfessionalId,
                clock: _clock
        );
    }
}