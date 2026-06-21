using Application.Appointments.Common.Queries;
using Application.Common.Abstractions.Context;
using Application.Common.Exceptions;

namespace Application.Appointments.GetDetails;

public sealed class Loader
    : IQueryContextLoader<Query, Context>
{
    private readonly IAppointmentQueries _queries;

    public Loader(IAppointmentQueries queries)
    {
        _queries = queries;
    }

    public async Task LoadAsync(
        Query request,
        Context context,
        CancellationToken ct
    )
    {
        var appointment = await _queries.GetAppointmentDetailsAsync(request.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        context.Appointment = appointment;
    }
}