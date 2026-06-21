using MediatR;

namespace Application.Calendar.RemoveException;

public sealed record Command(
    int StoreId,
    DateOnly Date
) : IRequest;