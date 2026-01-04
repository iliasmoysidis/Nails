using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Offerings;

public class CreateTests
{
    [Fact]
    public void Create_ShouleCreateOffering_WhenDataIsValid()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        var offering = Offering.Create(
            storeId: 1,
            name: "Haircut",
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        offering.StoreId.Should().Be(1);
        offering.Name.Should().Be("Haircut");
        offering.Price.Should().Be(Money.EUR(50));
        offering.Duration.Should().Be(Duration.FromMinutes(60));
        offering.IsDeleted.Should().Be(false);
        offering.CreatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void Create_ShouleTrhow_WhenNameIsEmpty()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        Action act = () => Offering.Create(
            storeId: 1,
            name: "  ",
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        act.Should().Throw<DomainException>().WithMessage("Service name cannot be empty.");
    }

    [Fact]
    public void Create_ShouleTrhow_WhenNameIsMoreThan200Characters()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        Action act = () => Offering.Create(
            storeId: 1,
            name: new string('a', 1000),
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        act.Should().Throw<DomainException>().WithMessage("Service name cannot exceed 200 characters.");
    }
}