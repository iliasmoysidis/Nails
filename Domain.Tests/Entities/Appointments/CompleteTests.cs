using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Appointments;

public class CompleteTests
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
    public void Complete_ShouldSetStatusToCompleted()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        clock.Advance(TimeSpan.FromMinutes(121));
        appointment.Complete(clock);

        appointment.IsCompleted.Should().Be(true);
        appointment.UpdatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void Complete_ShouldThrow_WhenAppointmentIsDeleted()
    {
        var (appointment, clock) = CreateFutureAppointment();

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
        var (appointment, clock) = CreateFutureAppointment();

        Action act = () => appointment.Complete(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("*Only confirmed appointments can be completed*");
    }

    [Fact]
    public void Complete_ShouldThrow_WhenTimeIsBeforeEndTime()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);
        clock.Advance(TimeSpan.FromMinutes(61));

        Action act = () => appointment.Complete(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Cannot complete appointments before its end time.");
    }
}