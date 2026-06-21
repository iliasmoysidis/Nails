namespace Application.Stores.Search;

public sealed record Query(
    string? Name,
    string? City,
    string? CountryCode
);