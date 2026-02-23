namespace Application.Commands.Appointments;

public sealed record ScheduleCommand(
    int UserId,
    int ProfessionalId,
    int OfferingId,
    int StoreId,
    DateTime StartAt,
    int Duration,
    decimal Price,
    string Currency,
    string? Notes
);

