using Application.Calendar.Common.Queries;

namespace Application.Calendar.GetAvailability;

public sealed class GetCalendarAvailabilityHandler
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