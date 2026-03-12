namespace Application.DTO.Assignments;

public sealed record ProfessionalOfferingDTO(
    int Id,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes
);