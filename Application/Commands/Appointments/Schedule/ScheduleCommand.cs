namespace Application.Commands.Appointments;

public sealed record ScheduleCommand(
    int UserId,
    int ProfessionalId,
    int OfferingId,
    int StoreId,
    DateTime StartAt,
    string? Notes
);

