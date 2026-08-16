using MediatR;

namespace Application.Assignments.Remove;

public sealed record RemoveAssignmentsCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;