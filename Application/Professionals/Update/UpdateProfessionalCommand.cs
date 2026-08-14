using MediatR;

namespace Application.Professionals.Update;

public sealed record UpdateProfessionalCommand(
    int ProfessionalId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
) : IRequest;