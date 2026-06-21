using MediatR;

namespace Application.Professionals.Register;

public sealed record Command(
    string FirstName,
    string LastName,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber,
    string TaxCountryCode,
    string TaxIdNumber
) : IRequest<int>;