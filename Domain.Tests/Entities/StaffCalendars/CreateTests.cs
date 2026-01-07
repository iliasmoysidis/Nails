using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities.StaffCalendars;

public class CreateTests
{
    [Fact]
    public void Create_ShouldCreateStaffCalendar_WhenDataIsValid()
    {
        var staff = StaffCalendar.Create(storeId: 1, professionalId: 2);

        staff.StoreId.Should().Be(1);
        staff.ProfessionalId.Should().Be(2);
    }
}