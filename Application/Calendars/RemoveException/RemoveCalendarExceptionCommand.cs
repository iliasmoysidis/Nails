using MediatR;

namespace Application.Calendars.RemoveException;

public sealed record RemoveCalendarExceptionCommand(
    int StoreId,
    DateOnly Date
) : IRequest;