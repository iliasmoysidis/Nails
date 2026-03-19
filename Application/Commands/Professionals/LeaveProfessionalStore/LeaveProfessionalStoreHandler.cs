using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreHandler
    : IRequestHandler<LeaveProfessionalStoreCommand>
{
    private readonly LeaveProfessionalStoreContext _ctx;
    private readonly IClock _clock;

    public LeaveProfessionalStoreHandler(
        LeaveProfessionalStoreContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public async Task Handle(
        LeaveProfessionalStoreCommand command,
        CancellationToken ct)
    {
        _ctx.Staff.RemoveProfessional(
                professionalId: command.ProfessionalId,
                clock: _clock
        );
    }
}