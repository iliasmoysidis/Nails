namespace Application.DTO.Appointment;

public sealed record AppointmentListItemDTO(
    int Id,
    int UserId,
    string UserName,
    string OfferingName,
    DateTime StartAt,
    DateTime EndAt,
    string Status,
    string Notes
);