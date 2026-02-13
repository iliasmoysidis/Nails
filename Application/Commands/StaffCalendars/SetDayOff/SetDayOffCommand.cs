namespace Application.Commands.StaffCalendars;

public sealed record SetDayOffCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
);