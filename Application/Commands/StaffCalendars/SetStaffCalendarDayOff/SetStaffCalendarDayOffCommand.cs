namespace Application.Commands.StaffCalendars;

public sealed record SetStaffCalendarDayOffCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
);