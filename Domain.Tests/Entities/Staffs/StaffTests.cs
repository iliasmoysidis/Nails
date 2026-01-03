using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class StaffTests
{

    [Fact]
    public void Create_ShouldAddInitialOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.IsOwner(10).Should().Be(true);
    }

    [Fact]
    public void EnsureOwner_ShouldNotThrow_WhenProfessionalIsOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.EnsureOwner(10)).Should().NotThrow();
    }

    [Fact]
    public void EnsureOwner_ShouldThrow_WhenProfessionalIsNotOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.EnsureOwner(9)).Should().Throw<DomainException>().WithMessage("Only an owner can perform this action.");
    }

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

    [Fact]
    public void AddOwner_ShouldNotThrow_WhenAddingNewOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.AddOwner(10, 11)).Should().NotThrow();

        staff.IsOwner(11).Should().Be(true);
    }

    [Fact]
    public void AddOwner_ShouldThrow_WhenProfessionalAlreadyOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.AddOwner(10, 10)).Should().Throw<DomainException>().WithMessage("Professional is already an owner of this store.");
    }

    [Fact]
    public void RemoveOwner_ShouldNotThrow_WhenRemovingAnOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddOwner(10, 11);

        staff.Invoking(s => s.RemoveOwner(10, 11)).Should().NotThrow();

        staff.IsOwner(11).Should().Be(false);
    }

    [Fact]
    public void RemoveOwner_ShouldThrow_WhenRemovingLastOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.Invoking(s => s.RemoveOwner(10, 10)).Should().Throw<DomainException>().WithMessage("Cannot remove last owner.");
    }

    [Fact]
    public void RemoveOwner_ShouldThrow_WhenProfessionalIsNotOwner()
    {
        var staff = Staff.Create(storeId: 1, initialOwnerId: 10);

        staff.AddOwner(10, 11);

        staff.Invoking(s => s.RemoveOwner(10, 12)).Should().Throw<DomainException>().WithMessage("Professional is not an owner of this store.");
    }

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