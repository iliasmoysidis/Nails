using Domain.Entities;
using Domain.ValueObjects.Time;
using FluentAssertions;
using Domain.Tests.Fakes;
using Domain.Exceptions;

namespace Appointments;

public class RescheduleTests
{
    [Fact]
    public void Reschedule_ShouldThrow_WhenAppointmentAlreadyStarted()
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

    [Fact]
    public void Reschedule_ShouldUpdateTimes_WhenAppointmentNotStarted()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(24);
        var endAt = startAt.AddHours(1);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);

        var newStartAt = startAt.AddHours(1);
        var newEndAt = newStartAt.Add(endAt - startAt);
        appointment.Reschedule(startAt: newStartAt, endAt: newEndAt, clock);

        appointment.StartAt.Should().Be(newStartAt);
        appointment.EndAt.Should().Be(newEndAt);
    }

    [Fact]
    public void Reschedule_ShouldThrow_WhenAppointmentIsCanceled()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);
        appointment.Cancel(clock);

        var newStartAt = startAt.AddHours(1);
        var newEndAt = newStartAt.Add(endAt - startAt);

        Action act = () => appointment.Reschedule(
            startAt: newStartAt,
            endAt: newEndAt,
            clock
        );

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Appointment cannot be modified.");
    }
}