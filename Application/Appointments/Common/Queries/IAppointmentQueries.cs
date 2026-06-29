using Application.Appointments.GetDetails;
using Application.Appointments.Common.DTO;
using Application.Common.DTO;

namespace Application.Appointments.Common.Queries;

public interface IAppointmentQueries
{
    Task<AppointmentDetailsDTO?> GetAppointmentDetailsAsync(int appointmentId, CancellationToken ct);

    Task<PagedResult<AppointmentListItemDTO>> GetProfessionalAppointmentsAsync(
        int professionalId,
        DateOnly? from,
        DateOnly? to,
        int? page,
        int? pageSize,
        CancellationToken ct
    );

    Task<PagedResult<AppointmentListItemDTO>> GetUserAppointmentsAsync(
        int userId,
        DateOnly? from,
        DateOnly? to,
        int? page,
        int? pageSize,
        CancellationToken ct
    );

    Task<PagedResult<AppointmentListItemDTO>> GetStoreAppointmentsAsync(
        int storeId,
        DateOnly? from,
        DateOnly? to,
        int? page,
        int? pageSize,
        CancellationToken ct
    );
}
