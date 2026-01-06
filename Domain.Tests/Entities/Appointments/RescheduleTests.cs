using Domain.Entities;
using Domain.ValueObjects.Time;
using FluentAssertions;
using Domain.Tests.Fakes;
using Domain.Exceptions;
using Domain.ValueObjects.Finance;

namespace Domain.Tests.Entities.Appointments;

public class RescheduleTests
{
    private static (Appointment appointment, FakeClock clock)
    CreateFutureAppointment()
    {
        var baseTime = new UtcDateTime(
            new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc));

        var clock = new FakeClock(baseTime);

        var appointment = Appointment.Create(
            userId: 1,
            professionalId: 1,
            offeringId: 1,
            storeId: 1,
            price: Money.EUR(50),
            startAt: clock.Now.AddHours(1),
            duration: Duration.FromMinutes(60),
            clock);

        return (appointment, clock);
    }

    [Fact]
    public void Reschedule_ShouldWork_WhenAppointmentIsPending()
    {
        var (appointment, clock) = CreateFutureAppointment();
        var newStartAt = clock.Now.AddHours(24);

        appointment.Reschedule(
            startAt: newStartAt,
            clock: clock);

        appointment.StartAt.Should().Be(newStartAt);
        appointment.UpdatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void Reschedule_ShouldWork_WhenAppointmentIsConfirmed()
    {
        var (appointment, clock) = CreateFutureAppointment();
        var newStartAt = clock.Now.AddHours(24);

        appointment.Confirm(clock);

        appointment.Reschedule(
            startAt: newStartAt,
            clock: clock);

        appointment.StartAt.Should().Be(newStartAt);
        appointment.UpdatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void Reschedule_ShouldThrow_WhenAppointmentIsDeleted()
    {
        var (appointment, clock) = CreateFutureAppointment();
        var newStartAt = clock.Now.AddHours(24);

        appointment.SoftDelete(clock);

        Action act = () => appointment.Reschedule(
            startAt: newStartAt,
            clock: clock
        );

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Appointment cannot be modified.");
    }

    [Fact]
    public void Reschedule_ShouldThrow_WhenAppointmentIsCanceled()
    {
        var (appointment, clock) = CreateFutureAppointment();
        var newStartAt = clock.Now.AddHours(24);

        appointment.Confirm(clock);
        appointment.Cancel(clock);

        Action act = () => appointment.Reschedule(
            startAt: newStartAt,
            clock: clock
        );

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Appointment cannot be modified.");
    }

    [Fact]
    public void Reschedule_ShouldThrow_WhenAppointmentIsCompleted()
    {
        var (appointment, clock) = CreateFutureAppointment();
        var newStartAt = clock.Now.AddHours(24);

        appointment.Confirm(clock);
        clock.Advance(TimeSpan.FromMinutes(121));
        appointment.Complete(clock);

        Action act = () => appointment.Reschedule(
            startAt: newStartAt,
            clock: clock
        );

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Appointment cannot be modified.");
    }

    [Fact]
    public void Reschedule_ShouldThrow_WhenAppointmentIsNoShow()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        clock.Advance(TimeSpan.FromMinutes(61));
        appointment.MarkAsNoShow(clock);

        Action act = () => appointment.Reschedule(
            startAt: clock.Now.AddHours(1),
            clock: clock
        );

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Appointment cannot be modified.");
    }

    [Fact]
    public void Reschedule_ShouldThrow_WhenAppointmentStartIsInThePast()
    {
        var (appointment, clock) = CreateFutureAppointment();
        var newStartAt = clock.Now.AddHours(-1);

        appointment.Confirm(clock);

        Action act = () => appointment.Reschedule(
            startAt: newStartAt,
            clock: clock
        );

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Appointment start time must be in the future.");
    }
}