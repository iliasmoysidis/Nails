namespace Application.Commands.StaffCalendars;

public sealed record AddStaffCalendarVacationCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
);