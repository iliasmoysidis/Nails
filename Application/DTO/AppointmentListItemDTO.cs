namespace Application.DTO;

public sealed record AppointmentListItemDTO(
    int AppointmentId,
    DateTime StartAt,
    DateTime EndAt,
    string Status,
    string ProfesionalName,
    string ServiceName
);