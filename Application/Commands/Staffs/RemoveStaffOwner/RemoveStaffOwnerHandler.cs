using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffOwnerHandler
    : IRequestHandler<RemoveStaffOwnerCommand>
{
    private readonly RemoveStaffOwnerContext _ctx;
    private readonly IClock _clock;

    public RemoveStaffOwnerHandler(
        RemoveStaffOwnerContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(
        RemoveStaffOwnerCommand command,
        CancellationToken ct)
    {
        _ctx.Staff.RemoveOwner(command.ProfessionalId, _clock);

        return Task.CompletedTask;
    }
}