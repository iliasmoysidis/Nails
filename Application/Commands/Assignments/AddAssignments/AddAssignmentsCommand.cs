using MediatR;

namespace Application.Commands.Assignments;

public sealed record AddAssignmentsCommand(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;