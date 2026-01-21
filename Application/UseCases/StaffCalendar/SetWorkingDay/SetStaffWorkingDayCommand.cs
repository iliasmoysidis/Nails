using Application.Abstractions;
using Domain.ValueObjects.Calendar;

namespace Application.StaffCalendar.SetWorkingDay;

public sealed record SetStaffWorkingDayCommand(
    int StoreId,
    int ProfessionalId,
    WorkingDay WorkingDay
) : ICommand;