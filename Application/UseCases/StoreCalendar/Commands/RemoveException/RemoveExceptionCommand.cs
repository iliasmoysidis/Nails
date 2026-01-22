using Application.Abstractions;

namespace Application.UseCases.StoreCalendar.Commands.RemoveException;

public sealed record RemoveExceptionCommand(int StoreId, DateOnly Date) : ICommand;