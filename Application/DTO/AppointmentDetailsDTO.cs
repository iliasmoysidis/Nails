namespace Application.DTO;

public sealed record AppointmentDetailsDTO(
    int AppointmentId,
    DateTime StartAt,
    DateTime EndAt,
    string Status,
    string ProfessionalName,
    string ProfessionalEmail,
    string ServiceName,
    decimal Price,
    string Currency,
    string? Notes
);