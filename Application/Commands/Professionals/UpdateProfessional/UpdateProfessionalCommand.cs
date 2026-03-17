using MediatR;

namespace Application.Commands.Professionals;

public sealed record UpdateProfessionalCommand(
    int ProfessionalId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
) : IRequest;