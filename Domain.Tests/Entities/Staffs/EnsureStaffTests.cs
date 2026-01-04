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

        staff.Invoking(s => s.EnsureStaff(11)).Should().NotThrow();
    }

    [Fact]
    public void EnsureStaff_ShouldThrow_WhenProfessionalIsNotStaff()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.EnsureStaff(11)).Should().Throw().WithMessage("Professional does not work for the store.");
    }
}