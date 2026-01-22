using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.Commands.SetWorkingDay;

public sealed record SetWorkingDayCommand(int StoreId, WorkingDay WorkingDay) : ICommand;