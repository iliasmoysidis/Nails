namespace Application.Commands.StaffCalendars;

public sealed record RemoveExceptionCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date
);