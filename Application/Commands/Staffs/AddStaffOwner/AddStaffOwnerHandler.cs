using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Staffs;

public sealed class AddStaffOwnerHandler
    : IRequestHandler<AddStaffOwnerCommand>
{
    private readonly AddStaffOwnerContext _ctx;
    private readonly IClock _clock;

    public AddStaffOwnerHandler(
        AddStaffOwnerContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(
        AddStaffOwnerCommand command,
        CancellationToken ct)
    {
        _ctx.Staff.AddOwner(command.ProfessionalId, _clock);

        return Task.CompletedTask;
    }
}