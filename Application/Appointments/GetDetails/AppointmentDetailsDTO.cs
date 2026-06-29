namespace Application.Appointments.GetDetails;


public sealed record BasicStoreDTO(
    int Id,
    string Name
);

public sealed record BasicUserDTO(
    int Id,
    string FirstName,
    string LastName,
    string Email
);

public sealed record BasicProfessionalDTO(
    int Id,
    string FirstName,
    string LastName,
    string Email
);

public sealed record BasicOfferingDTO(
    int Id,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes
);

public sealed record AppointmentDetailsDTO(
    int Id,
    BasicStoreDTO Store,
    BasicUserDTO User,
    BasicProfessionalDTO Professional,
    BasicOfferingDTO Offering,
    DateTime StartAt,
    DateTime EndAt,
    string Status,
    string Notes
);
