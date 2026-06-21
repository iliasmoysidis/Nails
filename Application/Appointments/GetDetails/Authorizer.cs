using Application.Roster.Common.Queries;
using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;

namespace Application.Appointments.GetDetails;

public sealed class Authorizer
    : IAuthorizer<Query>
{
    private readonly IRequestContext _requestContext;
    private readonly Context _queryContext;
    private readonly IStaffQueries _staffQueries;

    public Authorizer(
        IRequestContext requestContext,
        Context queryContext,
        IStaffQueries staffQueries
    )
    {
        _requestContext = requestContext;
        _queryContext = queryContext;
        _staffQueries = staffQueries;
    }

    public async Task AuthorizeAsync(
        Query request,
        CancellationToken ct
    )
    {
        var appointment = _queryContext.Appointment
            ?? throw new InvalidOperationException("Appointment context not loaded.");

        var isStaff = await _staffQueries.IsStaffMemberAsync(
            storeId: appointment.StoreId,
            professionalid: _requestContext.ActorId,
            ct: ct
        );

        var isClient = appointment.UserId == _requestContext.ActorId;

        if (!isStaff && !isClient)
            throw new ApplicationLayerForbiddenException("Not allowed to view this appointment.");
    }
}