namespace Api.Assignments.Requests;

public sealed record AddAssignmentsRequest(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
);
