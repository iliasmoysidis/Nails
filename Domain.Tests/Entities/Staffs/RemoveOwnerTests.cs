using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class RemoveOwnerTests
{
    [Fact]
    public void RemoveOwner_ShouldNotThrow_WhenRemovingAnOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddOwner(10, 11);
        staff.RemoveOwner(10, 11);

        staff.IsOwner(11).Should().Be(false);
    }

    [Fact]
    public void RemoveOwner_ShouldThrow_WhenRemovingLastOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        Action act = () => staff.RemoveOwner(10, 10);

        act.Should().Throw<DomainException>().WithMessage("Cannot remove last owner.");
    }

    [Fact]
    public void RemoveOwner_ShouldThrow_WhenProfessionalIsNotOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddOwner(10, 11);

        Action act = () => staff.RemoveOwner(10, 12);

        act.Should().Throw<DomainException>().WithMessage("Professional is not an owner of this store.");
    }
}