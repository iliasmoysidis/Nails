using Domain.Common;
using MediatR;

namespace Application.Roster.Terminate;

public sealed class TerminateStaffHandler
    : IRequestHandler<TerminateStaffCommand>
{
    private readonly TerminateStaffContext _ctx;
    private readonly IClock _clock;

    public TerminateStaffHandler(TerminateStaffContext ctx, IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(
        TerminateStaffCommand command,
        CancellationToken ct
    )
    {
        _ctx.EmploymentTermination.Terminate(_clock);

        return Task.CompletedTask;
    }
}
