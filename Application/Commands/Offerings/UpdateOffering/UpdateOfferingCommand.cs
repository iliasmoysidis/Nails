using MediatR;

namespace Application.Commands.Offerings;

public sealed record UpdateOfferingCommand(
    int StoreId,
    int OfferingId,
    string? Name,
    decimal? Price,
    int? DurationMinutes,
    string? Description
) : IRequest;