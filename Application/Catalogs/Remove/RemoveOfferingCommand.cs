using MediatR;

namespace Application.Catalogs.Remove;

public sealed record RemoveOfferingCommand(
    int StoreId,
    int OfferingId
) : IRequest;