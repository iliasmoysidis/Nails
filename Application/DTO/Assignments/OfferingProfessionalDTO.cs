namespace Application.DTO.Assignments;

public sealed record OfferingProfessionalDTO(
    int ProfessionalId,
    string FullName,
    string Email,
    string Phone
);