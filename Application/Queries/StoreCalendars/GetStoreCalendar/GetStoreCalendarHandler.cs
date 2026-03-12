using Application.Abstractions.Queries;
using Application.DTO.Calendar;

namespace Application.Queries.StoreCalendars;

public sealed class GetStoreCalendarHandler
{
    private readonly IStoreCalendarQueries _queries;

    public GetStoreCalendarHandler(IStoreCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<CalendarDayDTO>> Handle(
        GetStoreCalendarQuery query,
        CancellationToken ct
    )
    {
        return await _queries.GetStoreCalendarAsync(
            storeId: query.StoreId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}