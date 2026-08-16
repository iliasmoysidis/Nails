namespace Api.Assignments.Requests;

public sealed record GetOfferingsByProfessionalRequest(
    int StoreId,
    int? Page,
    int? Limit
);
