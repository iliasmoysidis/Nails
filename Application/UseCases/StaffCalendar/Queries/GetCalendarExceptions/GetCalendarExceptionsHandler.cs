using Application.Abstractions;
using Application.Exceptions;
using Application.Policies;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Queries.GetCalendarExceptions;

public sealed class GetCalendarExceptionsHandler
    : IQueryHandler<GetCalendarExceptionsQuery, IReadOnlyCollection<CalendarException>>
{
    private readonly IStaffCalendarRepository _repo;
    private readonly AuthorizationPolicy _policy;
    private readonly ActorContextFactory _factory;

    public GetCalendarExceptionsHandler(IStaffCalendarRepository repo, ActorContextFactory factory, AuthorizationPolicy policy)
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
        var calendar = await _repo.GetAsync(query.StoreId, query.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found");

        var actor = await _factory.CreateAsync(query.StoreId, ct);

        _policy.EnsureCanViewExceptions(actor, query.ProfessionalId);

        return calendar.GetExceptions();
    }
}