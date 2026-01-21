using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.AddException;

public sealed record AddExceptionCommand(int StoreId, CalendarException Exception) : ICommand;