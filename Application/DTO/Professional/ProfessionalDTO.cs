namespace Application.DTO.Professional;

public sealed record ProfessionalDTO(
    int Id,
    string FullName,
    string Email,
    string Phone
);