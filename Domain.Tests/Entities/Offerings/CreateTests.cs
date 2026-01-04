using Domain.Entities;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Time;

namespace Domain.Tests.Entities.Offerings;

public class CreateTests
{
    [Fact]
    public void Create_ShouleCreateOffering_WhenDataIsValid()
    {
        var clock = new FakeClock(UtcDateTime.Now());

        // var offering = Offering.Create(
        //     storeId: 1,
        //     name: "Haircut"

        // );
    }
}