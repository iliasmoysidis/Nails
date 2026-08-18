using MediatR;

namespace Application.Catalogs.Update;

public sealed record UpdateOfferingCommand(
    int StoreId,
    int OfferingId,
    string? Name,
    decimal? Price,
    int? DurationMinutes,
    string? Description
) : IRequest;