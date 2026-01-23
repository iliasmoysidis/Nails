using Application.Abstractions;
using Application.Contexts;
using Application.Exceptions;
using Application.Policies;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StoreCalendar.Queries.GetCalendarExceptions;

public sealed class GetCalendarExceptionsHandler
    : IQueryHandler<GetCalendarExceptionsQuery, IReadOnlyCollection<CalendarException>>
{
    private readonly IStoreCalendarRepository _repo;
    private readonly ActorContextFactory _factory;
    private readonly AuthorizationPolicy _policy;

    public GetCalendarExceptionsHandler(
        IStoreCalendarRepository repo,
        AuthorizationPolicy policy,
        ActorContextFactory factory
    )
    {
        _repo = repo;
        _factory = factory;
        _policy = policy;
    }

    public async Task<IReadOnlyCollection<CalendarException>> Handle(
        GetCalendarExceptionsQuery query,
        CancellationToken ct
    )
    {
        var actor = await _factory.CreateAsync(query.StoreId, ct);
        _policy.EnsureIsStoreOwner(actor);

        var calendar = await _repo.GetAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerException("Store calendar not found.");

        return calendar.GetExceptions();
    }
}