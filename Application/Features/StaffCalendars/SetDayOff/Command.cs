using MediatR;

namespace Application.Features.StaffCalendars.SetDayOff;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day
) : IRequest;