namespace Api.Appointments.Requests;

public sealed record CreateAppointmentRequest(
    int UserId,
    int ProfessionalId,
    int OfferingId,
    int StoreId,
    DateTime StartAt,
    string? Notes
);
