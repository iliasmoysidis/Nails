namespace Application.Assignments.GetProfessionalsByOffering;

public sealed record ProfessionalSummaryDTO(
    int ProfessionalId,
    string FullName,
    string Email,
    string Phone
);