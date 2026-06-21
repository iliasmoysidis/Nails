namespace Application.Assignments.GetProfessionalOfferings;

public sealed record ProfessionalOfferingDTO(
    int Id,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes
);