using MediatR;

namespace Application.Assignments.Add;

public sealed record AddAssignmentsCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;