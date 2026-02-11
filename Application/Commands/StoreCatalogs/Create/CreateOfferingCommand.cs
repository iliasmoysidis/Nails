namespace Application.Commands.Offerings;

public sealed record CreateOfferingCommand(
    int StoreId,
    string Name,
    decimal Price,
    string Currency,
    int DurationMinutes,
    string? Description
);