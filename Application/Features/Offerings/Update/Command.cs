using MediatR;

namespace Application.Features.Offerings.Update;

public sealed record Command(
    int StoreId,
    int OfferingId,
    string? Name,
    decimal? Price,
    int? DurationMinutes,
    string? Description
) : IRequest;