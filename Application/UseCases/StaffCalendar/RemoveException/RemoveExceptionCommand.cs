using Application.Abstractions;

namespace Application.UseCases.StaffCalendar.RemoveException;

public sealed record RemoveExceptionCommand(int StoreId, int ProfessionalId, DateOnly Date) : ICommand;