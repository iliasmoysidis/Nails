namespace Application.Commands.Professionals;

public sealed record UpdateProfileCommand(
    int ProfessionalId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
);