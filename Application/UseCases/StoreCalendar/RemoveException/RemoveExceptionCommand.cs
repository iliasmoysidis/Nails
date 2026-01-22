using Application.Abstractions;

namespace Application.UseCases.StoreCalendar.RemoveException;

public sealed record RemoveExceptionCommand(int StoreId, DateOnly Date) : ICommand;