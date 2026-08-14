namespace Application.Stores.Search;

public sealed record SearchStoresQuery(
    string? Name,
    string? City,
    string? CountryCode,
    int? Page,
    int? Limit
);
