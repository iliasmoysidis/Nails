using Application.Abstractions.Authorization;
using Application.Abstractions.Queries;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Queries.StaffCalendars;

public sealed class GetStaffCalendarExceptionAuthorizer
    : IAuthorizer<GetStaffCalendarExceptionsQuery>
{
    private readonly IRequestContext _context;
    private readonly IStaffQueries _queries;

    public GetStaffCalendarExceptionAuthorizer(
        IRequestContext context,
        IStaffQueries queries
    )
    {
        _context = context;
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        GetStaffCalendarExceptionsQuery request,
        CancellationToken ct
    )
    {
        var isStaff = await _queries.IsStaffMemberAsync(
            storeId: request.StoreId,
            professionalid: request.ProfessionalId,
            ct: ct
        );

        if (!isStaff)
            throw new ApplicationLayerNotFoundException("Staff access required.");
    }
}