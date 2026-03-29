using Application.DTO.Calendar;
using MediatR;

namespace Application.Features.StaffCalendars.SetWorkingDay;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;