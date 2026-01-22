using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.Commands.StoreCalendar.SetWorkingDay;

public sealed record SetWorkingDayCommand(int StoreId, WorkingDay WorkingDay) : ICommand;