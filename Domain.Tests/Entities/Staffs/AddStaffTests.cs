using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class AddStaffTests
{
    [Fact]
    public void AddStaff_ShouldNotThrow_WhenAddingStaff()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.AddStaff(10, 11)).Should().NotThrow();

        staff.IsStaff(11).Should().Be(true);
    }

    [Fact]
    public void AddStaff_ShouldThrow_WhenAddingExistingStaff()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddStaff(10, 11);

        staff.Invoking(s => s.AddStaff(10, 11)).Should().Throw<DomainException>().WithMessage("Professional already works for the store.");
    }
}