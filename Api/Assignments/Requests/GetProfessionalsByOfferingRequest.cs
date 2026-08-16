namespace Api.Assignments.Requests;

public sealed record GetProfessionalsByOfferingRequest(
    int StoreId,
    int? Page,
    int? Limit
);
