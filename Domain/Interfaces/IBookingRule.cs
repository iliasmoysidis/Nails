using Domain.Services.Booking;
using Domain.ValueObjects.Time;

namespace Domain.Interfaces;

public interface IBookingRule
{
    void EnsureSatisfied(BookingContext ctx, int offeringId, int professionalId, UtcDateTime startAt, int? excludeAppointmentId = null);
}