using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.Queries.GetDailyAvailability;

public sealed class GetDailyAvailabilityHandler
    : IQueryHandler<GetDailyAvailabilityQuery, IReadOnlyCollection<TimeRange>>
{
    private readonly IStoreCalendarRepository _repo;

    public GetDailyAvailabilityHandler(IStoreCalendarRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyCollection<TimeRange>> Handle(GetDailyAvailabilityQuery query, CancellationToken ct)
    {
        var calendar = await _repo.GetAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerException("Store calendar not found.");

        return calendar.GetWorkingTimeRanges(query.Date);
    }
}