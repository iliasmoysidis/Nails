using Application.DTO.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed record AddSpecialAvailabilityCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
);