using Application.DTO.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed record AddStaffCalendarSpecialAvailabilityCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);