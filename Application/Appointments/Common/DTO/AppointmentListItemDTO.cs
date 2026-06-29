namespace Application.Appointments.Common.DTO;

public sealed record AppointmentListItemDTO(
    int Id,

    int StoreId,
    string StoreName,

    int UserId,
    string UserName,

    int ProfessionalId,
    string ProfessionalName,

    int OfferingId,
    string OfferingName,

    DateTime StartAt,
    DateTime EndAt,

    decimal Price,
    string Currency,
    
    string Status,
    string Notes
);
