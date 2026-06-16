using MediatR;

namespace Application.Features.Offerings.Create;

public sealed record Command(
    int StoreId,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
) : IRequest<int>;