using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed record RemoveStoreCalendarExceptionCommand(
    int StoreId,
    DateOnly Date
) : IRequest;