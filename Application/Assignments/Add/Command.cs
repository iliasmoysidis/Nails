using MediatR;

namespace Application.Assignments.Add;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    IReadOnlyCollection<int> OfferingIds
) : IRequest;