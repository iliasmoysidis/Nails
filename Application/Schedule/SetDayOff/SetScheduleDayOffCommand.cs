using MediatR;

namespace Application.Schedule.SetDayOff;

public sealed record SetScheduleDayOffCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
) : IRequest;