using Application.Abstractions.Queries;
using Application.Abstractions.Repositories;
using Application.DTO.StaffCalendar;
using Application.Exceptions;
using Application.Guards;

namespace Application.Queries.StaffCalendars;

public sealed class GetStaffCalendarExceptionsHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IStaffRepository _repo;
    private readonly IStaffCalendarQueries _queries;

    public GetStaffCalendarExceptionsHandler(
        AuthorizationGuard auth,
        IStaffRepository repo,
        IStaffCalendarQueries queries
    )
    {
        _auth = auth;
        _repo = repo;
        _queries = queries;
    }

    public async Task<IReadOnlyCollection<StaffCalendarExceptionDTO>> Handle(
        GetStaffCalendarExceptionsQuery query,
        CancellationToken ct
    )
    {
        var staff = await _repo.GetByStoreIdAsync(query.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureStaffMember(staff);

        return await _queries.GetExceptionsAsync(
            storeId: query.StoreId,
            professionalId: query.ProfessionalId,
            from: query.From,
            to: query.To,
            ct: ct
        );
    }
}