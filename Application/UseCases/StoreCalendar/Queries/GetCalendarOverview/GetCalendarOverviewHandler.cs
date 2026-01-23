using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Repositories;


namespace Application.UseCases.StoreCalendar.Queries.GetCalendarOverview;

public sealed class GetCalendarOverviewHandler : IQueryHandler<GetCalendarOverviewQuery, CalendarOverviewDTO>
{
    private readonly IStoreCalendarRepository _repo;

    public GetCalendarOverviewHandler(IStoreCalendarRepository repo)
    {
        _repo = repo;
    }

    public async Task<CalendarOverviewDTO> Handle(
        GetCalendarOverviewQuery query,
        CancellationToken ct
    )
    {
        var calendar = await _repo.GetAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerException("Store calendar not found.");

        var ranges = calendar.GetWorkingTimeRanges(query.Date);

        return new CalendarOverviewDTO(
            ranges.Any(),
            ranges
        );
    }
}