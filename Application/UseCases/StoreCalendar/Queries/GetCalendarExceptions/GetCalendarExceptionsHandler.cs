using System.Security.Authentication.ExtendedProtection;
using Application.Abstractions;
using Application.Exceptions;
using Application.Policies.Interfaces;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.Queries.GetCalendarExceptions;

public sealed class GetCalendarExceptionsHandler
    : IQueryHandler<GetCalendarExceptionsQuery, IReadOnlyCollection<CalendarException>>
{
    private readonly IStoreCalendarRepository _repo;
    private readonly IStoreOwnerAccessPolicy _policy;
    private readonly ICurrentUser _currentUser;

    public GetCalendarExceptionsHandler(
        IStoreCalendarRepository repo,
        IStoreOwnerAccessPolicy policy,
        ICurrentUser currentUser
    )
    {
        _repo = repo;
        _policy = policy;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<CalendarException>> Handle(
        GetCalendarExceptionsQuery query,
        CancellationToken ct
    )
    {
        var calendar = await _repo.GetAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerException("Store calendar not found.");

        await _policy.EnsureIsOwnerAsync(_currentUser.UserId, query.StoreId, ct);

        return calendar.GetExceptions();
    }
}