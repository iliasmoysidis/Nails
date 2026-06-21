namespace Application.Professionals.GetDetails;

public sealed record ProfessionalDTO(
    int Id,
    string FullName,
    string Email,
    string Phone
);