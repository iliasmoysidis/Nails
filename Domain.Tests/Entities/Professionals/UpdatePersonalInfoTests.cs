using Domain.Entities;
using Domain.Exceptions;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Identity;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Domain.Tests.Entities.Professionals;

public class UpdatePersonalInfoTests
{
    private static (Professional professional, FakeClock clock) CreateProfessional()
    {
        var baseTime = new UtcDateTime(new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc));

        var clock = new FakeClock(baseTime);

        var professional = Professional.Create(
            fullName: FullName.From("John", "Doe"),
            email: Email.From("fake@gmail.com"),
            phone: Phone.From("+30", "69972276320"),
            taxIdNumber: TaxIdentificationNumber.From("EL", "140617777"),
            clock: clock
        );

        return (professional, clock);
    }

    [Fact]
    public void UpdatePersonalInfo_ShouldUpdatePersonalInfo_WhenTheDataIsValid()
    {
        var (professional, clock) = CreateProfessional();

        clock.Advance(TimeSpan.FromDays(1));
        professional.UpdatePersonalInfo(clock, FullName.From("Nick", "Smith"), Phone.From("+30", "6976196485"));

        professional.FullName.Should().Be(FullName.From("Nick", "Smith"));
        professional.Phone.Should().Be(Phone.From("+30", "6976196485"));
        professional.UpdatedAt.Should().Be(clock.Now);
    }

    [Fact]
    public void UpdatePersonalInfo_ShouldThrow_WhenProfessionalIsDeleted()
    {
        var (professional, clock) = CreateProfessional();

        professional.SoftDelete(clock);

        Action act = () => professional.UpdatePersonalInfo(clock, FullName.From("Nick", "Smith"), Phone.From("+30", "6976196485"));

        act.Should().Throw<DomainException>().WithMessage("Cannot modify a deactivated user.");
    }
}