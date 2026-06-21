using MediatR;

namespace Application.Calendar.AddHoliday;

public sealed record Command(
    int StoreId,
    DateOnly Date
) : IRequest;