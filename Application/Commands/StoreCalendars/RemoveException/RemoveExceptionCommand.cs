namespace Application.Commands.StoreCalendars;

public sealed record RemoveExceptionCommand(int StoreId, DateOnly Date);