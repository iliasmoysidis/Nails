using Application.Abstractions;
using Application.Exceptions;
using Application.Repositories;
using Domain.ValueObjects.Calendar;

namespace Application.UseCases.StaffCalendar.Queries.GetCalendarExceptions;

public sealed class GetCalendarExceptionsHandler
    : IQueryHandler<GetCalendarExceptionsQuery, IReadOnlyCollection<CalendarException>>
{
    private readonly IStaffCalendarRepository _repo;
    private readonly ICurrentUser _currentUser;

    public GetCalendarExceptionsHandler(IStaffCalendarRepository repo, ICurrentUser currentUser)
    {
        _repo = repo;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<CalendarException>> Handle(
        GetCalendarExceptionsQuery query,
        CancellationToken ct
    )
    {
        var calendar = await _repo.GetAsync(query.StoreId, query.ProfessionalId, ct)
            ?? throw new ApplicationLayerException("Staff calendar not found");

        var staff = await _repo.GetStaffAsync(query.StoreId, ct);

        if (!staff.IsOwner(_currentUser.UserId) && query.ProfessionalId != _currentUser.UserId)
            throw new ApplicationLayerException("Not authorized.");

        return calendar.GetExceptions();
    }
}