using MediatR;

namespace Application.Schedules.RemoveException;

public sealed class RemoveScheduleExceptionHandler
    : IRequestHandler<RemoveScheduleExceptionCommand>
{
    private readonly RemoveScheduleExceptionContext _ctx;

    public RemoveScheduleExceptionHandler(
        RemoveScheduleExceptionContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        RemoveScheduleExceptionCommand command,
        CancellationToken ct)
    {
        _ctx.ProfessionalAvailability.RemoveException(command.Date);

        return Task.CompletedTask;
    }
}
