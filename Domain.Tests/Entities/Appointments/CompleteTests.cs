using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Appointments;

public class CompleteTests
{
    [Fact]
    public void Complete_ShouldSetStatusToCompleted()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: Money.EUR(50), startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);

        clock.Advance(endAt - clock.Now);

        appointment.Complete(clock);

        appointment.IsCompleted.Should().Be(true);
        appointment.UpdatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void Complete_ShouldThrow_WhenAppointmentIsDeleted()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: Money.EUR(50), startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);
        appointment.SoftDelete(clock);

        Action act = () => appointment.Complete(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*deleted appointment*");
    }

    [Fact]
    public void Complete_ShouldThrow_WhenStatusIsNotConfirmed()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: Money.EUR(50), startAt: startAt, endAt: endAt, clock);

        Action act = () => appointment.Complete(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Only confirmed appointments can be completed*");
    }

    [Fact]
    public void Complete_ShouldThrow_WhenTimeIsBeforeEndTime()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: Money.EUR(50), startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);
        clock.Advance(endAt - clock.Now - TimeSpan.FromMinutes(1));

        Action act = () => appointment.Complete(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Cannot complete appointments before its end time.");
    }
}