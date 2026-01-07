using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Appointments;

public class CreateTests
{
    [Fact]
    public void Create_ShouldCreateAppointment_WhenDataIsValid()
    {
        var baseTime = new UtcDateTime(
        new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc));
        var clock = new FakeClock(baseTime);

        var startAt = clock.Now.AddHours(1);
        var duration = Duration.FromMinutes(60);

        var appointment = Appointment.Create(
            userId: 1,
            professionalId: 2,
            offeringId: 3,
            storeId: 4,
            price: Money.EUR(50),
            startAt: startAt,
            duration: duration,
            clock);

        appointment.UserId.Should().Be(1);
        appointment.ProfessionalId.Should().Be(2);
        appointment.OfferingId.Should().Be(3);
        appointment.StoreId.Should().Be(4);
        appointment.BookedPrice.Should().Be(Money.EUR(50));
        appointment.StartAt.Should().Be(startAt);
        appointment.Duration.Should().Be(duration);
        appointment.IsDeleted.Should().Be(false);
        appointment.CreatedAt.Should().Be(clock.Now);
        appointment.Status.Should().Be(AppointmentStatus.PendingConfirmation);
        appointment.EndAt.Should().Be(startAt.Add(duration.Value));

    }

    [Fact]
    public void Create_ShouldThrow_WhenStartIsInThePast()
    {
        var baseTime = new UtcDateTime(
        new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc));
        var clock = new FakeClock(baseTime);

        var startAt = clock.Now.AddHours(-1);
        var duration = Duration.FromMinutes(60);

        Action act = () => Appointment.Create(
            userId: 1,
            professionalId: 2,
            offeringId: 3,
            storeId: 4,
            price: Money.EUR(50),
            startAt: startAt,
            duration: duration,
            clock);

        act.Should().Throw<DomainException>().WithMessage("Appointment start time must be in the future.");
    }
}