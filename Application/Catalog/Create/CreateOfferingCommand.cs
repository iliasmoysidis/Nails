using MediatR;

namespace Application.Catalog.Create;

public sealed record CreateOfferingCommand(
    int StoreId,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
) : IRequest<int>;