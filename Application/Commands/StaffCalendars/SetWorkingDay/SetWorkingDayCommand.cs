using Application.DTO.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed record SetWorkingDayCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);