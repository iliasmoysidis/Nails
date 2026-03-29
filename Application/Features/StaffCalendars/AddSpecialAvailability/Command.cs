using Application.DTO.Calendar;
using MediatR;

namespace Application.Features.StaffCalendars.AddSpecialAvailability;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;