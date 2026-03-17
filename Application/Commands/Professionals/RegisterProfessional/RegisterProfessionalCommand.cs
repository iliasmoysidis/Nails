using MediatR;

namespace Application.Commands.Professionals;

public sealed record RegisterProfessionalCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber,
    string TaxCountryCode,
    string TaxIdNumber
) : IRequest<int>;