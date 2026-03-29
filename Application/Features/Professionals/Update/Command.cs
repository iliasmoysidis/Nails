using MediatR;

namespace Application.Features.Professionals.Update;

public sealed record Command(
    int ProfessionalId,
    string? FirstName,
    string? LastName,
    string? PhoneCountryCode,
    string? PhoneNumber
) : IRequest;