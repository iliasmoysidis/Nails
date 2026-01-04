using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class RemoveStaffTests
{
    [Fact]
    public void RemoveStaff_ShouldNotThrow_WhenRemovingStaff()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddStaff(10, 11);

        staff.Invoking(s => s.RemoveStaff(10, 11)).Should().NotThrow();

        staff.IsStaff(11).Should().Be(false);
    }

    [Fact]
    public void RemoveStaff_ShouldThrow_WhenRemovingProfessionalThatIsNotStaff()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.RemoveStaff(10, 11)).Should().Throw<DomainException>().WithMessage("Professional does not work for the store.");
    }
}