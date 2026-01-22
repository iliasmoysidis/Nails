using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.Commands.StoreCalendar.AddException;

public sealed record AddExceptionCommand(int StoreId, CalendarException Exception) : ICommand;