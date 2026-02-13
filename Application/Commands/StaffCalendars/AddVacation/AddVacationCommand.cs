namespace Application.Commands.StaffCalendars;

public sealed record AddVacationCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
);