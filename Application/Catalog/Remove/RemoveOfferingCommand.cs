using MediatR;

namespace Application.Catalog.Remove;

public sealed record RemoveOfferingCommand(
    int StoreId,
    int OfferingId
) : IRequest;