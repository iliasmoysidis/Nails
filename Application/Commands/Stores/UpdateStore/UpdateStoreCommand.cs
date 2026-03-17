using MediatR;

namespace Application.Commands.Stores;

public sealed record UpdateStoreCommand(
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