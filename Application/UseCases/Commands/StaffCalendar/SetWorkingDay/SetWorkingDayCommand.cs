using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.Commands.StaffCalendar.SetWorkingDay;

public sealed record SetWorkingDayCommand(
    int StoreId,
    int ProfessionalId,
    WorkingDay WorkingDay
) : ICommand;