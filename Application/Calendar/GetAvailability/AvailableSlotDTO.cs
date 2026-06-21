namespace Application.Calendar.GetAvailability;

public sealed record AvailableSlotDTO(
    DateTime StartAt,
    DateTime EndAt
);