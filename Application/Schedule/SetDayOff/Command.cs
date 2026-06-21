using MediatR;

namespace Application.Schedule.SetDayOff;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
) : IRequest;