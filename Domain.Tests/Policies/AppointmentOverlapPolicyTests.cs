using Domain.Entities;
using Domain.Exceptions;
using Domain.Policies;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Policies;

public class AppointmentOverlapPolicyTests
{
    private static Appointment CreateConfirmedAppointment(int id, UtcDateTime start, UtcDateTime end)
    {
        var clock = new FakeClock(
            start.AddHours(-1)
        );

        var appointment = Appointment.Create(
            userId: 1,
            professionalId: 1,
            offeringId: 1,
            storeId: 1,
            price: 50,
            startAt: start,
            endAt: end,
            clock
        );

        appointment.Confirm(clock);

        typeof(Appointment).GetProperty(nameof(Appointment.Id))!.SetValue(appointment, id);

        return appointment;
    }

    [Fact]
    public async Task EnsureNoConflictAsync_ShouldNotThrow_WhenNoAppointmentsExist()
    {
        var repo = new FakeAppointmentRepository(Array.Empty<Appointment>());
        var policy = new AppointmentOverlapPolicy(repo);

        var start = UtcDateTime.From(new DateTime(2025, 1, 1, 10, 0, 0));
        var end = start.AddHours(1);

        await policy.Invoking(p => p.EnsureNoConflictAsync(1, start, end)).Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureNoCoflictAsync_ShouldThrow_WhenAppointmentConflicts()
    {
        var appointment = CreateConfirmedAppointment(
            id: 1,
            start: UtcDateTime.From(new DateTime(2025, 1, 1, 10, 0, 0)),
            end: UtcDateTime.From(new DateTime(2025, 1, 1, 11, 0, 0)));

        var repo = new FakeAppointmentRepository([appointment]);
        var policy = new AppointmentOverlapPolicy(repo);

        var start = UtcDateTime.From(new DateTime(2025, 1, 1, 10, 30, 0));
        var end = start.AddHours(1);

        await policy.Invoking(p => p.EnsureNoConflictAsync(1, start, end)).Should().ThrowAsync<DomainException>().WithMessage("Professional already has an appointment at this time.");
    }
}