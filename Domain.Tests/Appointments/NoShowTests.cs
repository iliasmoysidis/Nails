using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Appointments;

public class NoShowTests
{
    [Fact]
    public void NoShow_ShouldSetStatusToNoShow()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);

        clock.Advance(startAt - clock.Now + TimeSpan.FromMinutes(1));

        appointment.MarkAsNoShow(clock);

        appointment.IsNoShow.Should().Be(true);
        appointment.UpdatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void NoShow_ShouldThrow_WhenAppointmentIsDeleted()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.SoftDelete(clock);

        Action act = () => appointment.MarkAsNoShow(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*mark as no-show a deleted appointment*");
    }

    [Fact]
    public void NoShow_ShouldThrow_WhenStatusIsNotConfirmed()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        Action act = () => appointment.MarkAsNoShow(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Only confirmed appointments can be marked as no-show*");
    }

    [Fact]
    public void NoShow_ShouldThrow_WhenTimeIsBeforeStart()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);

        Action act = () => appointment.MarkAsNoShow(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*no-show before appointment start*");
    }
}