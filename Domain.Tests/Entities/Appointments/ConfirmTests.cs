using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Appointments;

public class ConfirmTests
{
    [Fact]
    public void Confirm_ShouldSetStatusToConfirmed()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        clock.Advance(TimeSpan.FromMinutes(10));
        appointment.Confirm(clock);

        appointment.IsConfirmed.Should().Be(true);
        appointment.UpdatedAt.Should().Be(clock.Now);
    }



    [Fact]
    public void Confirm_ShouldThrow_WhenAppointmentIsDeleted()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.SoftDelete(clock);

        Action act = () => appointment.Confirm(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Cannot confirm deleted appointment.");
    }

    [Fact]
    public void Confirm_ShouldThrow_WhenAlreadyConfirmed()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        appointment.Confirm(clock);

        Action act = () => appointment.Confirm(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Cannot confirm appointment.*");
    }

    [Fact]
    public void Confirm_ShouldThrow_WhenStartIsInThePast()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);

        var appointment = Appointment.Create(userId: 1, professionalId: 1, offeringId: 1, storeId: 1, price: 50, startAt: startAt, endAt: endAt, clock);

        clock.Advance(TimeSpan.FromMinutes(90));

        Action act = () => appointment.Confirm(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Cannot confirm appointments in the past.");
    }
}