using MediatR;

namespace Application.Catalogs.Create;

public sealed record CreateOfferingCommand(
    int StoreId,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
) : IRequest<int>;