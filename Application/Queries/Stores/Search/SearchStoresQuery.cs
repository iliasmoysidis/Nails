namespace Application.Queries.Stores;

public sealed record SearchStoresQuery(
    string? Name,
    string? City,
    string? CountryCode
);