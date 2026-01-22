using Application.Abstractions;

namespace Application.UseCases.Commands.StoreCalendar.RemoveException;

public sealed record RemoveExceptionCommand(int StoreId, DateOnly Date) : ICommand;