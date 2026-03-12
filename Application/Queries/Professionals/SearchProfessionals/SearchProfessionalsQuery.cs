namespace Application.Queries.Professionals;

public sealed record SearchProfessionalsQuery(
    string? Name,
    int? OfferingId,
    string? City,
    int? StoreId
);