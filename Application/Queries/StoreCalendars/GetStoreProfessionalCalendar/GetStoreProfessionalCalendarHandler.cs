using Application.Abstractions.Queries;
using Application.Abstractions.Repositories;
using Application.DTO.Calendar;
using Application.Exceptions;
using Application.Guards;

namespace Application.Queries.StoreCalendars;

public sealed class GetStoreProfessionalCalendarHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffRepository _repo;
    private readonly IStoreCalendarQueries _queries;

    public GetStoreProfessionalCalendarHandler(
        AuthorizationGuard auth,
        IStaffRepository repo,
        IStoreCalendarQueries queries
    )
    {
        _auth = auth;
        _repo = repo;
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<CalendarDayDTO>> Handle(
        GetStoreProfessionalCalendarQuery query,
        CancellationToken ct
    )
    {
        var staff = await _repo.GetByStoreIdAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureCanViewStoreProfessionalCalendar(query.ProfessionalId, staff);

        return await _queries.GetProfessionalCalendarAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}