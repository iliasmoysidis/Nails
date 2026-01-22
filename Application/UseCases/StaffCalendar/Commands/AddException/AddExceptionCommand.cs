using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Commands.AddException;

public sealed record AddExceptionCommand(
    int StoreId,
    int ProfessionalId,
    CalendarException exception
) : ICommand;