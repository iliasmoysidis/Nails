using MediatR;

namespace Application.Professionals.Update;

public sealed record Command(
    int ProfessionalId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
) : IRequest;