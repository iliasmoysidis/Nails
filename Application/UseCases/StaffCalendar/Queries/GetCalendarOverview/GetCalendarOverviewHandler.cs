using Application.Abstractions;
using Application.DTO;
using Application.Exceptions;
using Application.Repositories;

namespace Application.UseCases.StaffCalendar.Queries.GetCalendarOverview;

public sealed class GetCalendarOverviewHandler
    : IQueryHandler<GetCalendarOverviewQuery, CalendarOverviewDTO>
{
    private readonly IStaffCalendarRepository _repo;

    public GetCalendarOverviewHandler(IStaffCalendarRepository repo)
    {
        _repo = repo;
    }

    public async Task<CalendarOverviewDTO> Handle(
        GetCalendarOverviewQuery query,
        CancellationToken ct
    )
    {
        var calendar = await _repo.GetAsync(query.StoreId, query.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found.");

        var ranges = calendar.GetWorkingTimeRanges(query.Date);

        return new CalendarOverviewDTO(ranges.Any(), ranges);
    }
}