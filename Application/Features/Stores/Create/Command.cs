using MediatR;

namespace Application.Features.Stores.Create;

public sealed record Command(
    int ProfessionalId,
    string Name,
    string Street,
    string City,
    string PostalCode,
    string State,
    string CountryCode,
    string Email,
    string PhoneCountryCode,
    string PhoneNumber,
    string TaxCountryCode,
    string TaxNumber
) : IRequest<int>;