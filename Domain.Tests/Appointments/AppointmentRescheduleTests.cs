using Domain.Entities;
using Domain.ValueObjects.Time;
using FluentAssertions;
using Domain.Tests.Fakes;
using Domain.Exceptions;

namespace Appointments;

public class AppointmentRescheduleTests
{
    [Fact]
    public void RescheduleShouldThrowWhenAppointmentAlreadyStarted()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: clock.Now.AddMinutes(15), endAt: clock.Now.AddMinutes(45), clock);

        appointment.Confirm(clock);

        clock.Advance(TimeSpan.FromMinutes(20));

        Action act = () => appointment.Reschedule(
            startAt: clock.Now.AddHours(1),
            endAt: clock.Now.AddHours(2),
            clock
        );

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*already started*");
    }
}