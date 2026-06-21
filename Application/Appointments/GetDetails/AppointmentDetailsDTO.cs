namespace Application.Appointments.GetDetails;

public sealed record AppointmentDetailsDTO(
    int Id,
    int StoreId,
    int UserId,
    int ProfessionalId,
    int OfferingId,
    DateTime StartAt,
    DateTime EndAt,
    string Status,
    decimal Price,
    string Currency,
    string Notes
);