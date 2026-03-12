namespace Application.DTO.Calendar;

public sealed record AvailableSlotDTO(
    DateTime StartAt,
    DateTime EndAt
);