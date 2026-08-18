using MediatR;

namespace Application.Schedules.RemoveException;

public sealed record RemoveScheduleExceptionCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;