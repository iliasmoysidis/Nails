using MediatR;

namespace Application.Schedules.SetDayOff;

public sealed record SetScheduleDayOffCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
) : IRequest;