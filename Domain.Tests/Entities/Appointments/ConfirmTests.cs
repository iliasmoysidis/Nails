using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Appointments;

public class ConfirmTests
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
    public void Confirm_ShouldSetStatusToConfirmed()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);

        appointment.IsConfirmed.Should().Be(true);
        appointment.UpdatedAt.Should().Be(clock.Now);
    }



    [Fact]
    public void Confirm_ShouldThrow_WhenAppointmentIsDeleted()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.SoftDelete(clock);

        Action act = () => appointment.Confirm(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Appointment is deleted.");
    }

    [Fact]
    public void Confirm_ShouldThrow_WhenAlreadyConfirmed()
    {
        var (appointment, clock) = CreateFutureAppointment();

        appointment.Confirm(clock);

        Action act = () => appointment.Confirm(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Cannot confirm appointment.*");
    }

    [Fact]
    public void Confirm_ShouldThrow_WhenStartIsInThePast()
    {
        var (appointment, clock) = CreateFutureAppointment();

        clock.Advance(TimeSpan.FromMinutes(61));

        Action act = () => appointment.Confirm(clock);

        act.Should()
            .Throw<DomainException>()
            .WithMessage("Cannot confirm appointments in the past.");
    }
}