namespace Api.Assignments.Requests;

public sealed record RemoveAssignmentsRequest(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
);
