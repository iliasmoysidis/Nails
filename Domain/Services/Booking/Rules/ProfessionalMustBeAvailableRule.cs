using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking.Rules;

public sealed class ProfessionalMustBeAvailableRule : IBookingRule
{
    public void EnsureSatisfied(BookingContext ctx, int offeringId, int professionalId, UtcDateTime startAt, int? excludeAppointmentId = null)
    {
        var offering = ctx.StoreCatalog.GetOffering(offeringId)
            ?? throw new DomainException("Service not found.");
        var endAt = startAt.Add(offering.Duration.Value);

        if (!ctx.StaffCalendar.IsProfessionalAvailable(startAt, endAt))
            throw new DomainException("Professional is unavailable at the requested time.");
    }
}