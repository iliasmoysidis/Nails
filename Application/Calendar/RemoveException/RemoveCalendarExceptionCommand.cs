using MediatR;

namespace Application.Calendar.RemoveException;

public sealed record RemoveCalendarExceptionCommand(
    int StoreId,
    DateOnly Date
) : IRequest;