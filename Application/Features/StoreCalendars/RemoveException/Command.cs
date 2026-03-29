using MediatR;

namespace Application.Features.StoreCalendars.RemoveException;

public sealed record Command(
    int StoreId,
    DateOnly Date
) : IRequest;