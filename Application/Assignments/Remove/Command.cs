using MediatR;

namespace Application.Assignments.Remove;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;