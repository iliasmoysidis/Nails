using Application.DTO.Calendar;
using MediatR;

namespace Application.Commands.StaffCalendars;

public sealed record AddStaffCalendarSpecialAvailabilityCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;