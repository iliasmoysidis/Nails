using MediatR;

namespace Application.Catalog.Update;

public sealed record UpdateOfferingCommand(
    int StoreId,
    int OfferingId,
    string? Name,
    decimal? Price,
    int? DurationMinutes,
    string? Description
) : IRequest;