using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Services.Booking.Rules;

public sealed class StoreMustBeOpenRule : IBookingRule
{
    public void EnsureSatisfied(BookingContext ctx, int offeringId, int professionalId, UtcDateTime startAt, int? excludeAppointmentId = null)
    {
        var offering = ctx.StoreCatalog.GetOffering(offeringId)
            ?? throw new DomainException("Service not found.");
        var endAt = startAt.Add(offering.Duration.Value);

        if (!ctx.StoreCalendar.IsOpenAt(startAt) || !ctx.StoreCalendar.IsOpenAt(endAt.AddMinutes(-1)))
            throw new DomainException("Store is closed at the requested time.");
    }
}