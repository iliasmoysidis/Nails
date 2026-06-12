using MediatR;

namespace Application.Features.Offerings.Delete;

public sealed record Command(
    int StoreId,
    int OfferingId
) : IRequest;