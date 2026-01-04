using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Offerings;

public class DeactivateTests
{
    [Fact]
    public void Deactivate_ShouldSoftDeleteOffering()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        var offering = Offering.Create(
            storeId: 1,
            name: "Haircut",
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        offering.Deactivate(clock);

        offering.IsDeleted.Should().Be(true);
    }

    [Fact]
    public void Deactivate_ShouldThrow_WhenAlreadyDeleted()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        var offering = Offering.Create(
            storeId: 1,
            name: "Haircut",
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        offering.SoftDelete(clock);

        Action act = () => offering.Deactivate(clock);

        act.Should().Throw<DomainException>().WithMessage("Service is already deactivated.");
    }
}