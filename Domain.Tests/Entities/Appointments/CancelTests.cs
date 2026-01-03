using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Entities.Appointments;

public class CancelTest
{
    [Fact]
    public void Cancel_ShouldSetStatusToCanceled()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);
        appointment.Cancel(clock);

        appointment.IsCanceled.Should().Be(true);
        appointment.CanceledAt.Should().NotBeNull();
    }

    [Fact]
    public void Cancel_ShouldSetCanceledAt()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);


        clock.Advance(TimeSpan.FromMinutes(5));
        appointment.Cancel(clock);
        var cancelTime = clock.Now;

        appointment.CanceledAt.Should().Be(cancelTime);
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenAlreadyCanceled()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);
        appointment.Cancel(clock);

        Action act = () => appointment.Cancel(clock);

        act.Should()
        .Throw<DomainException>()
        .WithMessage("Appointment cannot be modified.");
    }
}