using MediatR;

namespace Application.Features.StoreCalendars.AddHoliday;

public sealed record Command(
    int StoreId,
    DateOnly Date
) : IRequest;