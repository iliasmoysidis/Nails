using Domain.Entities;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Professionals;

public class CreateTests
{
    [Fact]
    public void Create_ShouldCreateProfessional_WhenDataIsValid()
    {
        var baseTime = new UtcDateTime(
            new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc));

        var clock = new FakeClock(baseTime);

        var professional = Professional.Create(
            fullName: FullName.From("John", "Doe"),
            email: Email.From("fake@gmail.com"),
            phone: Phone.From("+30", "69972276320"),
            taxIdNumber: TaxIdentificationNumber.From("EL", "140617777"),
            clock: clock
        );

        professional.FullName.Should().Be(FullName.From("John", "Doe"));
        professional.Email.Should().Be(Email.From("fake@gmail.com"));
        professional.Phone.Should().Be(Phone.From("+30", "69972276320"));
        professional.TaxIdNumber.Should().Be(TaxIdentificationNumber.From("EL", "140617777"));
        professional.CreatedAt.Should().Be(clock.Now);
        professional.IsDeleted.Should().Be(false);
    }
}