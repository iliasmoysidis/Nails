using Application.Common.DTO;
using MediatR;

namespace Application.Schedules.AddSpecialAvailability;

public sealed record AddSpecialAvailabilityCommand(
    int StoreId,
    int ProfessionalId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;