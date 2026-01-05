using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Appointment : HistoricEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int OfferingId { get; private set; }
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }
    public UtcDateTime StartAt { get; private set; }
    public Duration Duration { get; private set; } = null!;
    public UtcDateTime EndAt => StartAt.Add(Duration.Value);
    public Money BookedPrice { get; private set; } = null!;
    public string? Notes { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public UtcDateTime? CanceledAt { get; private set; }


    public bool IsPending => Status == AppointmentStatus.PendingConfirmation;
    public bool IsConfirmed => Status == AppointmentStatus.Confirmed;
    public bool IsCompleted => Status == AppointmentStatus.Completed;
    public bool IsCanceled => Status == AppointmentStatus.Canceled;
    public bool IsNoShow => Status == AppointmentStatus.NoShow;
    public bool IsUpcoming(UtcDateTime now) => Status == AppointmentStatus.Confirmed && StartAt > now;
    public bool IsPast(UtcDateTime now) => EndAt < now;
    public bool IsInProgress(UtcDateTime now) => Status == AppointmentStatus.Confirmed && StartAt <= now && EndAt > now;

    private Appointment()
    {
        Status = AppointmentStatus.PendingConfirmation;
    }

    public static Appointment Create(
        int userId,
        int professionalId,
        int offeringId, int storeId,
        Money price,
        UtcDateTime startAt,
        Duration duration,
        IClock clock,
        string? notes = null
        )
    {
        EnsureStartIsAfterNow(startAt, clock);
        ValidateNotes(notes);

        var appointment = new Appointment
        {
            UserId = userId,
            OfferingId = offeringId,
            ProfessionalId = professionalId,
            StoreId = storeId,
            BookedPrice = price,
            StartAt = startAt,
            Duration = duration,
            Notes = notes?.Trim(),
            Status = AppointmentStatus.PendingConfirmation,
        };

        appointment.MarkAsCreated(clock);
        return appointment;
    }

    public void Confirm(IClock clock)
    {
        EnsureNotDeleted();

        if (Status != AppointmentStatus.PendingConfirmation)
            throw new DomainException($"Cannot confirm appointment. Current status is {Status}. Only pending appointments can be confirmed.");

        if (StartAt <= clock.Now)
            throw new DomainException("Cannot confirm appointments in the past.");

        Status = AppointmentStatus.Confirmed;
        MarkAsUpdated(clock);
    }

    public void Reschedule(UtcDateTime startAt, IClock clock)
    {
        EnsureIsMutable();
        EnsureStartIsAfterNow(startAt, clock);

        StartAt = startAt;
        MarkAsUpdated(clock);
    }

    public void Cancel(IClock clock, string? reason = null)
    {
        EnsureIsMutable();

        if (StartAt <= clock.Now)
            throw new DomainException("Cannot cancel an ongoing appointment.");

        if (reason != null)
        {
            Notes = string.IsNullOrWhiteSpace(Notes)
            ? $"Cancellation reason: {reason}"
            : $"{Notes}\nCancellation reason: {reason}";
        }

        Status = AppointmentStatus.Canceled;
        CanceledAt = clock.Now;
        MarkAsUpdated(clock);
    }

    public void Complete(IClock clock)
    {
        EnsureNotDeleted();

        if (Status != AppointmentStatus.Confirmed)
            throw new DomainException($"Cannot complete appointment. Current status is {Status}. Only confirmed appointments can be completed.");

        if (clock.Now < EndAt)
            throw new DomainException("Cannot complete appointment before its end time.");

        Status = AppointmentStatus.Completed;
        MarkAsUpdated(clock);
    }

    public void MarkAsNoShow(IClock clock)
    {
        EnsureNotDeleted();

        if (Status != AppointmentStatus.Confirmed)
            throw new DomainException($"Cannot mark as no-show. Current status is {Status}. Only confirmed appointments can be marked as no-show.");

        if (clock.Now < StartAt)
            throw new DomainException("Cannot mark as no-show before appointment start time.");

        Status = AppointmentStatus.NoShow;
        MarkAsUpdated(clock);
    }

    public void UpdateNotes(IClock clock, string? notes)
    {
        EnsureIsMutable();

        Notes = notes?.Trim();
        MarkAsUpdated(clock);
    }

    public void AdjustPrice(Money newPrice, string reason, IClock clock)
    {
        EnsureIsMutable();

        BookedPrice = newPrice;

        var adjustmentNote = $"Price adjusted to {newPrice}. Reason: {reason}";
        Notes = string.IsNullOrWhiteSpace(Notes)
            ? adjustmentNote
            : $"{Notes}\n{adjustmentNote}";

        MarkAsUpdated(clock);
    }

    public bool ConflictsWith(UtcDateTime start, UtcDateTime end)
    {
        if (IsDeleted) return false;

        if (Status is AppointmentStatus.Canceled or AppointmentStatus.Completed or AppointmentStatus.NoShow)
            return false;

        return start < EndAt && end > StartAt;
    }

    private static void ValidateNotes(string? notes = null)
    {
        if (notes != null && notes.Length > 500)
        {
            throw new DomainException("Notes cannot be more than 500 characters.");
        }
    }

    private void EnsureIsMutable()
    {
        if (IsDeleted || IsCompleted || IsCanceled || IsNoShow)
        {
            throw new DomainException("Appointment cannot be modified.");
        }
    }

    private static void EnsureStartIsAfterNow(UtcDateTime startAt, IClock clock)
    {
        if (startAt <= clock.Now)
            throw new DomainException("Appointment start time must be in the future.");
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Appointment is deleted.");
    }
}
