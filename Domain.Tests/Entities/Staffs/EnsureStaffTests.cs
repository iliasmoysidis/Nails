using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class EnsureStaffTests
{
    [Fact]
    public void EnsureStaff_ShouldNotThrow_WhenProfessionalIsStaff()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddStaff(10, 11);

        Action act = () => staff.EnsureStaff(11);

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureStaff_ShouldThrow_WhenProfessionalIsNotStaff()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        Action act = () => staff.EnsureStaff(11);

        act.Should().Throw().WithMessage("Professional does not work for the store.");
    }
}