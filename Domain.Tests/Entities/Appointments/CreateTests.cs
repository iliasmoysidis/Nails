using Domain.Entities;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Appointments;

public class CreateTests
{
    [Fact]
    public void Create_ShouldNotThrow_WhenDataIsValid()
    {
        var baseTime = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var clock = new FakeClock(UtcDateTime.From(baseTime));

        var startAt = clock.Now.AddHours(1);
        var endAt = startAt.AddHours(2);
        var appointment = Appointment.Create(userId: 1, professionalId: 2, offeringId: 3, storeId: 4, price: Money.EUR(50), startAt: startAt, endAt: endAt, clock);

        appointment.UserId.Should().Be(1);
        appointment.ProfessionalId.Should().Be(2);
        appointment.OfferingId.Should().Be(3);
        appointment.StoreId.Should().Be(4);
        appointment.BookedPrice.Should().Be(Money.EUR(50));
        appointment.StartAt.Should().Be(startAt);
        appointment.EndAt.Should().Be(endAt);
        appointment.IsDeleted.Should().Be(false);
        appointment.CreatedAt.Should().Be(clock.Now);
    }
}