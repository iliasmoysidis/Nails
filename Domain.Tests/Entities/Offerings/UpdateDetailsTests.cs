using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Offerings;

public class UpdateDetailsTests
{
    [Fact]
    public void UpdateDetails_ShouldNotThrow_WhenDataIsValid()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        var offering = Offering.Create(
            storeId: 1,
            name: OfferingName.Create("Haircut"),
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        clock.Advance(TimeSpan.FromHours(1));

        offering.UpdateDetails(
            clock: clock,
            name: OfferingName.Create("Low Taper Fade"),
            price: Money.EUR(100),
            duration: Duration.FromMinutes(120)
        );

        offering.Name.Should().Be(OfferingName.Create("Low Taper Fade"));
        offering.Price.Should().Be(Money.EUR(100));
        offering.Duration.Should().Be(Duration.FromMinutes(120));
        offering.UpdatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void UpdateDetails_ShouldThrow_WhenOfferingIsDeleted()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        var offering = Offering.Create(
            storeId: 1,
            name: OfferingName.Create("Haircut"),
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        offering.SoftDelete(clock);

        clock.Advance(TimeSpan.FromHours(1));

        Action act = () => offering.UpdateDetails(
            clock: clock,
            name: OfferingName.Create("Haircut"),
            price: Money.EUR(100),
            duration: Duration.FromMinutes(120)
        );

        act.Should().Throw<DomainException>().WithMessage("Cannot update inactive service.");
    }

    [Fact]
    public void UpdateDetails_ShouldThrow_WhenNameIsEmpty()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        var offering = Offering.Create(
            storeId: 1,
            name: OfferingName.Create("Haircut"),
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        clock.Advance(TimeSpan.FromHours(1));

        Action act = () => offering.UpdateDetails(
            clock: clock,
            name: OfferingName.Create("")
        );

        act.Should().Throw<DomainException>().WithMessage("Service name cannot be empty.");
    }

    [Fact]
    public void UpdateDetails_ShouldThrow_WhenNameLengthExceeds200Characters()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        var offering = Offering.Create(
            storeId: 1,
            name: OfferingName.Create("Haircut"),
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        clock.Advance(TimeSpan.FromHours(1));

        Action act = () => offering.UpdateDetails(
            clock: clock,
            name: OfferingName.Create(new string('a', 1000))
        );

        act.Should().Throw<DomainException>().WithMessage("Service name cannot exceed 200 characters.");
    }
}