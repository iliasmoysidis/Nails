using Application.Rosters.Common.Queries;
using Application.Common.Abstractions.Authorization;
using Application.Common.Contexts;
using Application.Common.Exceptions;
using Application.Rosters.Common.Repositories;

namespace Application.Appointments.GetDetails;

public sealed class GetAppointmentDetailsAuthorizer
    : IAuthorizer<GetAppointmentDetailsQuery>
{
    private readonly IRequestContext _requestContext;
    private readonly GetAppointmentDetailsContext _queryContext;
    private readonly IStaffQueries _staffQueries;
    private readonly IStaffRepository _staffRepo;

    public GetAppointmentDetailsAuthorizer(
        IRequestContext requestContext,
        GetAppointmentDetailsContext queryContext,
        IStaffQueries staffQueries,
        IStaffRepository staffRepo
    )
    {
        _requestContext = requestContext;
        _queryContext = queryContext;
        _staffQueries = staffQueries;
        _staffRepo = staffRepo;
    }

    public async Task AuthorizeAsync(
        GetAppointmentDetailsQuery request,
        CancellationToken ct
    )
    {
        var appointment = _queryContext.Appointment
            ?? throw new InvalidOperationException("Appointment context not loaded.");

        var isStaff = await _staffRepo.IsStaffMemberAsync(
            storeId: appointment.Store.Id,
            professionalid: _requestContext.ActorId,
            ct: ct
        );

        var isClient = appointment.User.Id == _requestContext.ActorId;

        if (!isStaff && !isClient)
            throw new ApplicationLayerForbiddenException("Not allowed to view this appointment.");
    }
}
