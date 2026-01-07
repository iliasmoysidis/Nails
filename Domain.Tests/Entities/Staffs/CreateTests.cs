using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class CreateTests
{
    [Fact]
    public void Create_ShouldAddInitialOwner_WhenDataIsValid()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.IsOwner(10).Should().Be(true);
    }
}