using MediatR;

namespace Application.Assignments.Add;

public sealed record AddAssignmentCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;