using Domain.Entities;
using Domain.Services.Booking;

namespace Application.Abstractions;

public interface IBookingRepository
{
    Task<BookingContext> LoadContextAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
    );

    Task<Appointment?> GetAppointmentAsync(int appointmentId, CancellationToken ct);

    void Add(Appointment appointment);
}