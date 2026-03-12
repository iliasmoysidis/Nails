using Application.Abstractions.Queries;
using Application.DTO.Calendar;

namespace Application.Queries.StoreCalendars;

public sealed class GetStoreProfessionalAvailabilityHandler
{
    private readonly IStoreCalendarQueries _queries;

    public GetStoreProfessionalAvailabilityHandler(IStoreCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AvailableSlotDTO>> Handle(
        GetStoreProfessionalAvailabilityQuery query,
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