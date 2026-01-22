using Application.Abstractions;

namespace Application.UseCases.Commands.StaffCalendar.RemoveException;

public sealed record RemoveExceptionCommand(int StoreId, int ProfessionalId, DateOnly Date) : ICommand;