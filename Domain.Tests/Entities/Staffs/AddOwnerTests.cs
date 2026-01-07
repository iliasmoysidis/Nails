using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class AddOwnerTests
{
    [Fact]
    public void AddOwner_ShouldNotThrow_WhenAddingNewOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddOwner(10, 11);

        staff.IsOwner(11).Should().Be(true);
    }

    [Fact]
    public void AddOwner_ShouldThrow_WhenProfessionalAlreadyOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        Action act = () => staff.AddOwner(10, 10);

        act.Should().Throw<DomainException>().WithMessage("Professional is already an owner of this store.");
    }
}