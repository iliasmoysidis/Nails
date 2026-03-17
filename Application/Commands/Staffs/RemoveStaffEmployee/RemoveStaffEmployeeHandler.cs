using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffEmployeeHandler
    : IRequestHandler<RemoveStaffEmployeeCommand>
{
    private readonly RemoveStaffEmployeeContext _ctx;
    private readonly IClock _clock;

    public RemoveStaffEmployeeHandler(
        RemoveStaffEmployeeContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(
        RemoveStaffEmployeeCommand command,
        CancellationToken ct)
    {
        _ctx.Assignments.RemoveProfessionalAssignments(command.ProfessionalId);

        _ctx.Staff.RemoveEmployee(command.ProfessionalId, _clock);

        return Task.CompletedTask;
    }
}