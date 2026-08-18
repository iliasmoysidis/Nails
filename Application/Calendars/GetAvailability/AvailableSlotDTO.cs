namespace Application.Calendars.GetAvailability;

public sealed record AvailableSlotDTO(
    DateTime StartAt,
    DateTime EndAt
);