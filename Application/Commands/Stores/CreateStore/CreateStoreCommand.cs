using MediatR;

namespace Application.Commands.Stores;

public sealed record CreateStoreCommand(
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