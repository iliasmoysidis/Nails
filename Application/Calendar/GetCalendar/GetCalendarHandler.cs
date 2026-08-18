using Application.Calendar.Common.DTO;
using Application.Calendar.Common.Queries;
using MediatR;

namespace Application.Calendar.GetCalendar;

public sealed class GetCalendarHandler
    : IRequestHandler<GetCalendarQuery, IReadOnlyCollection<CalendarDayDTO>>
{
    private readonly IStoreCalendarQueries _queries;

    public GetCalendarHandler(IStoreCalendarQueries queries)
    {
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<CalendarDayDTO>> Handle(
        GetCalendarQuery query,
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
