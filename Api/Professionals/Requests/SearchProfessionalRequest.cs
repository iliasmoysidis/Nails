namespace Api.Professionals.Requests;

public sealed record SearchProfessionalRequest(
    string? Name,
    int? OfferingId,
    string? City,
    int? StoreId
);
