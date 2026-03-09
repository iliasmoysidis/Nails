namespace Application.Commands.Assignments;

public sealed record RemoveAssignmentsCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
);