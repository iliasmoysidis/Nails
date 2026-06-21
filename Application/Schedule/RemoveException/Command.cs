using MediatR;

namespace Application.Schedule.RemoveException;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
) : IRequest;