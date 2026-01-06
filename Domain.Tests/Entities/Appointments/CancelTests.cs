using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Appointments;

public class CancelTests
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
    public void Cancel_ShouldSetStatusToCanceled()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        appointment.Cancel(clock);

        appointment.IsCanceled.Should().Be(true);
        appointment.CanceledAt.Should().Be(clock.Now);
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenAppointmentStarted()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        clock.Advance(TimeSpan.FromMinutes(61));

        Action act = () => appointment.Cancel(clock);

        act.Should()
        .Throw<DomainException>()
        .WithMessage("Cannot cancel an ongoing appointment.");
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenAlreadyCanceled()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        appointment.Cancel(clock);

        Action act = () => appointment.Cancel(clock);

        act.Should()
        .Throw<DomainException>()
        .WithMessage("Appointment cannot be modified.");
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenAppointmentIsDeleted()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.SoftDelete(clock);

        Action act = () => appointment.Cancel(clock);

        act.Should()
        .Throw<DomainException>()
        .WithMessage("Appointment cannot be modified.");
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenAppointmentIsCompleted()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        clock.Advance(TimeSpan.FromMinutes(121));
        appointment.Complete(clock);

        Action act = () => appointment.Cancel(clock);

        act.Should()
        .Throw<DomainException>()
        .WithMessage("Appointment cannot be modified.");
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenAppointmentIsNoShow()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        clock.Advance(TimeSpan.FromMinutes(61));
        appointment.MarkAsNoShow(clock);

        Action act = () => appointment.Cancel(clock);

        act.Should()
        .Throw<DomainException>()
        .WithMessage("Appointment cannot be modified.");
    }
}