using Application.Common.DTO;
using MediatR;

namespace Application.Schedule.AddSpecialAvailability;

public sealed record Command(
    int StoreId,
    int ProfessionalId,
    DateOnly Date,
    IReadOnlyCollection<TimeRangeDTO> TimeRanges
) : IRequest;