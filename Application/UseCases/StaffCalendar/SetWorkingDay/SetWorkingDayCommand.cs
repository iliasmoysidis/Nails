using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.SetWorkingDay;

public sealed record SetWorkingDayCommand(
    int StoreId,
    int ProfessionalId,
    WorkingDay WorkingDay
) : ICommand;