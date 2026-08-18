using Application.Calendars.Common.Queries;
using MediatR;

namespace Application.Calendars.GetAvailability;

public sealed class GetCalendarAvailabilityHandler
    : IRequestHandler<GetCalendarAvailabilityQuery, IReadOnlyCollection<AvailableSlotDTO>>
{
    private readonly IStoreCalendarQueries _queries;

    public GetCalendarAvailabilityHandler(IStoreCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AvailableSlotDTO>> Handle(
        GetCalendarAvailabilityQuery query,
        CancellationToken ct
    )
    {
        return await _queries.GetAvailableSlotsAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            offeringId: query.OfferingId,
            date: query.Date,
            ct: ct
        );
    }
}