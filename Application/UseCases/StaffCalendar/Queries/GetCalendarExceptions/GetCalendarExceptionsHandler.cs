using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Queries.GetCalendarExceptions;

public sealed class GetCalendarExceptionsHandler
    : IQueryHandler<GetCalendarExceptionsQuery, IReadOnlyCollection<CalendarException>>
{
    private readonly IStaffCalendarRepository _repo;
    private readonly IAuthorizationService _auth;

    public GetCalendarExceptionsHandler(IStaffCalendarRepository repo, IAuthorizationService auth)
    {
        _repo = repo;
        _auth = auth;
    }

    public async Task<IReadOnlyCollection<CalendarException>> Handle(
        GetCalendarExceptionsQuery query,
        CancellationToken ct
    )
    {
        var calendar = await _repo.GetAsync(query.StoreId, query.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found");

        await _auth.RequireProfessionalAccess(query.StoreId, query.ProfessionalId, ct);

        return calendar.GetExceptions();
    }
}