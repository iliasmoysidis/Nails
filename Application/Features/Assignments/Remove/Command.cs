using MediatR;

namespace Application.Features.Assignments.Remove;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;