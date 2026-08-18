using Application.Common.DTO;
using MediatR;

namespace Application.Schedules.SetWorkingDay;

public sealed record SetScheduleWorkingDayCommand(
    int StoreId,
    int ProfessionalId,
    DayOfWeek Day,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;