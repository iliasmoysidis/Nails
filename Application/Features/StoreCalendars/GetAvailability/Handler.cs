using Application.Abstractions.Queries;
using Application.DTO.Calendar;

namespace Application.Features.StoreCalendars.GetAvailability;

public sealed class Handler
{
    private readonly IStoreCalendarQueries _queries;

    public Handler(IStoreCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<AvailableSlotDTO>> Handle(
        Query query,
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