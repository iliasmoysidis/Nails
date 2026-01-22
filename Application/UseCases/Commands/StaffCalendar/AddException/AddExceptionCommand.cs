using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.Commands.StaffCalendar.AddException;

public sealed record AddExceptionCommand(
    int StoreId,
    int ProfessionalId,
    CalendarException exception
) : ICommand;