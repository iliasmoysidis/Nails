using MediatR;

namespace Application.Catalog.Remove;

public sealed record Command(
    int StoreId,
    int OfferingId
) : IRequest;