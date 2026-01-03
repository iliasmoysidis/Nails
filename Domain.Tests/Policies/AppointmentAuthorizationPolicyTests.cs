using Domain.Entities;
using Domain.Exceptions;
using Domain.Policies;
using Domain.Tests.Fakes;
using Domain.ValueObjects.Time;
using FluentAssertions;

namespace Policies;

public class AppointmentAuthorizationPolicyTests
{
    private readonly AppointmentAuthorizationPolicy _policy = new();

    private static Appointment CreateConfirmedAppointment(int userId, UtcDateTime start, FakeClock clock)
    {
        var appointment = Appointment.Create(
            userId: userId,
            professionalId: 1,
            offeringId: 1,
            storeId: 1,
            price: 50,
            startAt: start,
            endAt: start.AddHours(2),
            clock
        );

        appointment.Confirm(clock);

        return appointment;
    }

    private static Staff CreateStaffWithOwner(int storeId, int ownerId)
    {
        return Staff.Create(storeId, ownerId);
    }

    [Fact]
    public void EnsureCanModify_ShouldAllow_UserModifyOwnAppointment_MoreThan24HoursBefore()
    {
        var clock = new FakeClock(
            UtcDateTime.From(new DateTime(2025, 1, 1, 10, 0, 0))
        );

        var appointment = CreateConfirmedAppointment(
            userId: 10,
            start: clock.Now.AddHours(25),
            clock
        );

        var staff = CreateStaffWithOwner(1, 99);

        _policy.Invoking(p => p.EnsureCanModify(10, appointment, staff, clock.Now)).Should().NotThrow();
    }

    [Fact]
    public void EnsureCanModify_ShouldAllow_OwnerModifyAppointmentWithin24Hours()
    {
        var clock = new FakeClock(
            UtcDateTime.From(new DateTime(2025, 1, 1, 10, 0, 0))
        );

        var appointment = CreateConfirmedAppointment(
            userId: 10,
            start: clock.Now.AddHours(25),
            clock
        );

        var staff = CreateStaffWithOwner(1, 99);

        clock.Advance(TimeSpan.FromHours(2));

        _policy.Invoking(p => p.EnsureCanModify(99, appointment, staff, clock.Now)).Should().NotThrow();
    }

    [Fact]
    public void EnsureCanModify_ShouldThrow_WhenAgentIsNotUserOrOwner()
    {
        var clock = new FakeClock(
            UtcDateTime.From(new DateTime(2025, 1, 1, 10, 0, 0))
        );

        var appointment = CreateConfirmedAppointment(
            userId: 10,
            start: clock.Now.AddHours(25),
            clock
        );

        var staff = CreateStaffWithOwner(1, 99);

        _policy.Invoking(p => p.EnsureCanModify(50, appointment, staff, clock.Now)).Should().Throw<DomainException>().WithMessage("The user is not authorized to modify this appointment.");
    }

    [Fact]
    public void EnsureCanModify_ShouldThrow_WhenAppointmentAlreadyStarted()
    {
        var clock = new FakeClock(
            UtcDateTime.From(new DateTime(2025, 1, 1, 10, 0, 0))
        );

        var appointment = CreateConfirmedAppointment(
            userId: 10,
            start: clock.Now.AddHours(25),
            clock
        );

        var staff = CreateStaffWithOwner(1, 99);

        clock.Advance(TimeSpan.FromHours(26));

        _policy.Invoking(p => p.EnsureCanModify(10, appointment, staff, clock.Now)).Should().Throw<DomainException>().WithMessage("Appointment has already started.");
    }

    [Fact]
    public void EnsureCanModify_ShouldThrow_WhenUserModifyOwnAppointmentWithin24Hours()
    {
        var clock = new FakeClock(
            UtcDateTime.From(new DateTime(2025, 1, 1, 10, 0, 0))
        );

        var appointment = CreateConfirmedAppointment(
            userId: 10,
            start: clock.Now.AddHours(23),
            clock
        );

        var staff = CreateStaffWithOwner(1, 99);

        _policy.Invoking(p => p.EnsureCanModify(10, appointment, staff, clock.Now)).Should().Throw<DomainException>().WithMessage("Only an owner can modify appointments within 24 hours.");
    }
}