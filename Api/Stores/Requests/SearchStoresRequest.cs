namespace Api.Stores.Requests;

public sealed record SearchStoresRequest(
    string? Name,
    string? City,
    string? CountryCode,
    int? Page,
    int? Limit
);
