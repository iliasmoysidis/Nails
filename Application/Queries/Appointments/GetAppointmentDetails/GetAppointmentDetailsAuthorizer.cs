using Application.Abstractions.Authorization;
using Application.Abstractions.Queries;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Queries.Appointments;

public sealed class GetAppointmentDetailsAuthorizer
    : IAuthorizer<GetAppointmentDetailsQuery>
{
    private readonly IRequestContext _requestContext;
    private readonly GetAppointmentDetailsContext _queryContext;
    private readonly IStaffQueries _staffQueries;

    public GetAppointmentDetailsAuthorizer(
        IRequestContext requestContext,
        GetAppointmentDetailsContext queryContext,
        IStaffQueries staffQueries
    )
    {
        _requestContext = requestContext;
        _queryContext = queryContext;
        _staffQueries = staffQueries;
    }

    public async Task AuthorizeAsync(
        GetAppointmentDetailsQuery request,
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