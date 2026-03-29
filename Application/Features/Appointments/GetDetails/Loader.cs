using Application.Abstractions.Context;
using Application.Abstractions.Queries;
using Application.Exceptions;

namespace Application.Features.Appointments.GetDetails;

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