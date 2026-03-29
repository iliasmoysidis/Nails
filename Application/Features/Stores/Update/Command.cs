using MediatR;

namespace Application.Features.Stores.Update;

public sealed record Command(
    int StoreId,
    string? Name,
    string? Street,
    string? City,
    string? PostalCode,
    string? State,
    string? CountryCode,
    string? PhoneCountryCode,
    string? PhoneNumber
) : IRequest;