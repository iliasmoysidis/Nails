namespace Application.Professionals.Search;

public sealed record Query(
    string? Name,
    int? OfferingId,
    string? City,
    int? StoreId
);