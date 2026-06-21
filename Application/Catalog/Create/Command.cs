using MediatR;

namespace Application.Catalog.Create;

public sealed record Command(
    int StoreId,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
) : IRequest<int>;