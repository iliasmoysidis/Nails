using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Staffs;

public sealed class AddStaffEmployeeHandler
    : IRequestHandler<AddStaffEmployeeCommand>
{
    private readonly AddStaffEmployeeContext _ctx;
    private readonly IClock _clock;

    public AddStaffEmployeeHandler(
        AddStaffEmployeeContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(
        AddStaffEmployeeCommand command,
        CancellationToken ct)
    {
        _ctx.Staff.AddEmployee(command.ProfessionalId, _clock);

        return Task.CompletedTask;
    }
}