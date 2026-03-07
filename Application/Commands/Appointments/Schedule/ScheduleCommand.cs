using Domain.ValueObjects.Time;

namespace Application.Commands.Appointments;

public sealed record ScheduleCommand(
    int UserId,
    int ProfessionalId,
    int OfferingId,
    int StoreId,
    UtcDateTime StartAt,
    string? Notes
);

