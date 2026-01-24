using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.Queries.GetCalendarExceptions;

public sealed class GetCalendarExceptionsHandler
    : IQueryHandler<GetCalendarExceptionsQuery, IReadOnlyCollection<CalendarException>>
{
    private readonly IStoreCalendarRepository _repo;
    private readonly IAuthorizationService _auth;

    public GetCalendarExceptionsHandler(
        IStoreCalendarRepository repo,
        IAuthorizationService auth
    )
    {
        _repo = repo;
        _auth = auth;
    }

    public async Task<IReadOnlyCollection<CalendarException>> Handle(
        GetCalendarExceptionsQuery query,
        CancellationToken ct
    )
    {
        await _auth.RequireStoreOwner(query.StoreId, ct);

        var calendar = await _repo.GetAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerException("Store calendar not found.");

        return calendar.GetExceptions();
    }
}