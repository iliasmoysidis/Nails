using Application.Abstractions.Context;
using Application.Abstractions.Queries;
using Application.Exceptions;

namespace Application.Queries.Appointments;

public sealed class GetAppointmentDetailsLoader
    : IQueryContextLoader<GetAppointmentDetailsQuery, GetAppointmentDetailsContext>
{
    private readonly IAppointmentQueries _queries;

    public GetAppointmentDetailsLoader(IAppointmentQueries queries)
    {
        _queries = queries;
    }

    public async Task LoadAsync(
        GetAppointmentDetailsQuery request,
        GetAppointmentDetailsContext context,
        CancellationToken ct
    )
    {
        var appointment = await _queries.GetAppointmentDetailsAsync(request.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        context.Appointment = appointment;
    }
}