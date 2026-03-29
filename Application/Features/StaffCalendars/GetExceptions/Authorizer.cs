using Application.Abstractions.Authorization;
using Application.Abstractions.Queries;
using Application.Exceptions;

namespace Application.Features.StaffCalendars.GetExceptions;

public sealed class GetStaffCalendarExceptionAuthorizer
    : IAuthorizer<Query>
{
    private readonly IStaffQueries _queries;

    public GetStaffCalendarExceptionAuthorizer(IStaffQueries queries)
    {
        _queries = queries;
    }

    public async Task AuthorizeAsync(
        Query request,
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