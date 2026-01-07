using Domain.Entities;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
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
            name: OfferingName.Create("Haircut"),
            price: Money.EUR(50),
            duration: Duration.FromMinutes(60),
            clock: clock
        );

        offering.StoreId.Should().Be(1);
        offering.Name.Should().Be(OfferingName.Create("Haircut"));
        offering.Price.Should().Be(Money.EUR(50));
        offering.Duration.Should().Be(Duration.FromMinutes(60));
        offering.IsDeleted.Should().Be(false);
        offering.CreatedAt.Should().Be(clock.Now);
    }
}