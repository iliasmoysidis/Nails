using Application.Abstractions.Queries;
using Application.Abstractions.Repositories;
using Application.DTO.Appointment;
using Application.Exceptions;
using Application.Guards;

namespace Application.Queries.Appointments;

public sealed class GetAppointmentDetailsHandler
{
    private readonly AuthorizationGuard _auth;
    private readonly IAppointmentQueries _queries;
    private readonly IStaffRepository _staffRepo;

    public GetAppointmentDetailsHandler(
        AuthorizationGuard auth,
        IAppointmentQueries queries,
        IStaffRepository staffRepo
    )
    {
        _auth = auth;
        _queries = queries;
        _staffRepo = staffRepo;
    }

    public async Task<AppointmentDetailsDTO> Handle(GetAppointmentDetailsQuery query, CancellationToken ct)
    {
        var appointment = await _queries.GetAppointmentDetailsAsync(query.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var staff = await _staffRepo.GetByStoreIdAsync(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Staff not found.");

        _auth.EnsureCanViewAppointment(appointment, staff);

        return appointment;
    }
}