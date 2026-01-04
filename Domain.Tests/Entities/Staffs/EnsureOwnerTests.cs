using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Entities.Staffs;

public class EnsureOwnerTests
{
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
}