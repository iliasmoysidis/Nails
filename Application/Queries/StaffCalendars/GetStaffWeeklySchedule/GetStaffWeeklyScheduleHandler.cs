using Application.Abstractions.Queries;
using Application.Abstractions.Repositories;
using Application.DTO.StaffCalendar;
using Application.Exceptions;
using Application.Guards;

namespace Application.Queries.StaffCalendars;

public sealed class GetStaffWeeklyScheduleHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffRepository _repo;
    private readonly IStaffCalendarQueries _queries;

    public GetStaffWeeklyScheduleHandler(
        AuthorizationGuard auth,
        IStaffRepository repo,
        IStaffCalendarQueries queries
    )
    {
        _auth = auth;
        _repo = repo;
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffWorkingDayDTO>> Handle(GetStaffWeeklyScheduleQuery query, CancellationToken ct)
    {
        var staff = await _repo.GetByStoreIdAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureStaffMember(staff);

        return await _queries.GetWeeklyScheduleAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            ct: ct
        );
    }
}