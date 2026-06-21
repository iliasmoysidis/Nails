namespace Application.Assignments.GetOfferingProfessionals;

public sealed record OfferingProfessionalDTO(
    int ProfessionalId,
    string FullName,
    string Email,
    string Phone
);