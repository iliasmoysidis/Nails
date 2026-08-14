using MediatR;

namespace Application.Schedule.RemoveException;

public sealed record RemoveScheduleExceptionCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;