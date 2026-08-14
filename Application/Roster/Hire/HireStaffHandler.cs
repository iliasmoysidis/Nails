using MediatR;

namespace Application.Roster.Hire;

public sealed class HireStaffHandler
    : IRequestHandler<HireStaffCommand>
{
    private readonly HireStaffContext _ctx;

    public HireStaffHandler(HireStaffContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        HireStaffCommand command,
        CancellationToken ct
    )
    {
        _ctx.EmploymentCreation.Hire();

        return Task.CompletedTask;
    }
}
