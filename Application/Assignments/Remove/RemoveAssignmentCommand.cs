using MediatR;

namespace Application.Assignments.Remove;

public sealed record RemoveAssignmentCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;