using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Queries.GetDailyAvailability;

public sealed class GetDailyAvailabilityHandler
    : IQueryHandler<GetDailyAvailabilityQuery, IReadOnlyCollection<TimeRange>>
{
    private readonly IStaffCalendarRepository _repo;

    public GetDailyAvailabilityHandler(IStaffCalendarRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<TimeRange>> Handle(
        GetDailyAvailabilityQuery query,
        CancellationToken ct
    )
    {
        var calendar = await _repo.GetAsync(query.StoreId, query.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found.");

        return calendar.GetWorkingTimeRanges(query.Date);
    }
}