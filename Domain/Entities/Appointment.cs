using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Appointment
{
    public int Id { get; private set; }
    public int UserId { get; }
    public int ProfessionalId { get; }
    public int OfferingId { get; }
    public int StoreId { get; }

    public UtcDateTime StartAt { get; }
    public Duration Duration { get; }
    public UtcDateTime EndAt => StartAt.Add(Duration.Value);

    public Money Price { get; private set; }
    public Notes Notes { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public UtcDateTime? CanceledAt { get; private set; }

    public bool IsPending => Status == AppointmentStatus.PendingConfirmation;
    public bool IsConfirmed => Status == AppointmentStatus.Confirmed;
    public bool IsCompleted => Status == AppointmentStatus.Completed;
    public bool IsCanceled => Status == AppointmentStatus.Canceled;
    public bool IsNoShow => Status == AppointmentStatus.NoShow;

    public bool IsUpcoming(UtcDateTime now)
        => Status == AppointmentStatus.Confirmed && StartAt > now;

    public bool IsPast(UtcDateTime now)
        => EndAt < now;

    public bool IsInProgress(UtcDateTime now)
        => Status == AppointmentStatus.Confirmed && StartAt <= now && EndAt > now;

    public bool IsTerminal
        => Status is AppointmentStatus.Completed
           or AppointmentStatus.Canceled
           or AppointmentStatus.NoShow;

    private Appointment(
        int userId,
        int professionalId,
        int offeringId,
        int storeId,
        UtcDateTime startAt,
        Duration duration,
        Money price,
        Notes notes)
    {
        UserId = userId;
        ProfessionalId = professionalId;
        OfferingId = offeringId;
        StoreId = storeId;
        StartAt = startAt;
        Duration = duration;
        Price = price;
        Notes = notes;
        Status = AppointmentStatus.PendingConfirmation;
    }

    public static Appointment Create(
        int userId,
        int professionalId,
        int offeringId,
        int storeId,
        UtcDateTime startAt,
        Duration duration,
        Money price,
        Notes notes,
        IClock clock)
    {
        var now = clock.Now;

        if (startAt <= now)
            throw new ValidationException("Appointment start time must be in the future.");

        return new Appointment(
            userId,
            professionalId,
            offeringId,
            storeId,
            startAt,
            duration,
            price,
            notes);
    }

    public void Confirm(IClock clock)
    {
        EnsureStatus(AppointmentStatus.PendingConfirmation);

        var now = clock.Now;

        if (StartAt <= now)
            throw new InvariantException("Cannot confirm appointments in the past.");

        Status = AppointmentStatus.Confirmed;
    }

    public void Cancel(IClock clock, string? reason = null)
    {
        EnsureMutable();

        var now = clock.Now;

        if (StartAt <= now)
            throw new InvariantException("Cannot cancel an ongoing appointment.");

        Notes = Notes.Append("Cancellation reason", reason);

        Status = AppointmentStatus.Canceled;
        CanceledAt = now;
    }

    public void Complete(IClock clock)
    {
        EnsureStatus(AppointmentStatus.Confirmed);

        if (clock.Now < EndAt)
            throw new InvariantException("Cannot complete appointment before its end time.");

        Status = AppointmentStatus.Completed;
    }

    public void MarkAsNoShow(IClock clock)
    {
        EnsureStatus(AppointmentStatus.Confirmed);

        if (clock.Now < StartAt)
            throw new InvariantException("Cannot mark as no-show before appointment start time.");

        Status = AppointmentStatus.NoShow;
    }

    public void AdjustPrice(Money newPrice, string reason)
    {
        EnsureMutable();

        if (newPrice.Currency != Price.Currency)
            throw new InvariantException("Cannot change appointment currency.");

        Price = newPrice;
        Notes = Notes.Append("Price adjusted", $"{newPrice}. {reason}");
    }

    public bool ConflictsWith(UtcDateTime start, UtcDateTime end)
    {
        if (end <= start)
            throw new ValidationException("Invalid time range.");

        if (IsTerminal)
            return false;

        return start < EndAt && end > StartAt;
    }

    private void EnsureMutable()
    {
        if (IsTerminal)
            throw new StateException("Appointment cannot be modified.");
    }

    private void EnsureStatus(AppointmentStatus expected)
    {
        if (Status != expected)
            throw new StateException($"Invalid transition from {Status}.");
    }
}