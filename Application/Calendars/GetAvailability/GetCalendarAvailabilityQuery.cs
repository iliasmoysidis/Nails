using MediatR;

namespace Application.Calendars.GetAvailability;

public sealed record GetCalendarAvailabilityQuery(
    int StoreId,
    int ProfessionalId,
    int OfferingId,
    DateOnly Date
) : IRequest<IReadOnlyCollection<AvailableSlotDTO>>;