using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Appointments;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Appointment : HistoricEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int ProfessionalId { get; private set; }
    public int OfferingId { get; private set; }
    public int StoreId { get; private set; }
    public UtcDateTime StartAt { get; private set; }
    public Duration Duration { get; private set; }
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
    public bool IsUpcoming(UtcDateTime now) => Status == AppointmentStatus.Confirmed && StartAt > now;
    public bool IsPast(UtcDateTime now) => EndAt < now;
    public bool IsInProgress(UtcDateTime now) => Status == AppointmentStatus.Confirmed && StartAt <= now && EndAt > now;

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
        IClock clock
        )
    {
        EnsureStartIsAfterNow(startAt, clock);

        var appointment = new Appointment(
            userId: userId,
            professionalId: professionalId,
            offeringId: offeringId,
            storeId: storeId,
            startAt: startAt,
            duration: duration,
            price: price,
            notes: notes
        );

        appointment.MarkAsCreated(clock);
        return appointment;
    }

    public void Confirm(IClock clock)
    {
        EnsureActive();

        if (Status != AppointmentStatus.PendingConfirmation)
            throw new StateException(
                $"Cannot confirm appointment. Current status is {Status}.");

        if (StartAt <= clock.Now)
            throw new InvariantException("Cannot confirm appointments in the past.");

        Status = AppointmentStatus.Confirmed;
        MarkAsUpdated(clock);
    }

    public void Reschedule(UtcDateTime startAt, IClock clock)
    {
        EnsureActive();
        EnsureNotTerminal();
        EnsureStartIsAfterNow(startAt, clock);

        StartAt = startAt;
        MarkAsUpdated(clock);
    }

    public void Cancel(IClock clock, string? reason = null)
    {
        EnsureActive();
        EnsureNotTerminal();

        if (StartAt <= clock.Now)
            throw new InvariantException("Cannot cancel an ongoing appointment.");

        Notes = Notes.Append("Cancellation reason", reason);

        Status = AppointmentStatus.Canceled;
        CanceledAt = clock.Now;
        MarkAsUpdated(clock);
    }

    public void Complete(IClock clock)
    {
        EnsureActive();

        if (Status != AppointmentStatus.Confirmed)
            throw new StateException($"Cannot complete appointment. Current status is {Status}. Only confirmed appointments can be completed.");

        if (clock.Now < EndAt)
            throw new InvariantException("Cannot complete appointment before its end time.");

        Status = AppointmentStatus.Completed;
        MarkAsUpdated(clock);
    }

    public void MarkAsNoShow(IClock clock)
    {
        EnsureActive();

        if (Status != AppointmentStatus.Confirmed)
            throw new StateException($"Cannot mark as no-show. Current status is {Status}. Only confirmed appointments can be marked as no-show.");

        if (clock.Now < StartAt)
            throw new InvariantException("Cannot mark as no-show before appointment start time.");

        Status = AppointmentStatus.NoShow;
        MarkAsUpdated(clock);
    }

    public void UpdateNotes(IClock clock, string? notes)
    {
        EnsureActive();
        EnsureNotTerminal();

        Notes = Notes.From(notes);
        MarkAsUpdated(clock);
    }

    public void AdjustPrice(Money newPrice, string reason, IClock clock)
    {
        EnsureActive();
        EnsureNotTerminal();

        if (newPrice.Currency != Price.Currency)
            throw new InvariantException("Cannot change appointment currency.");

        Price = newPrice;
        Notes = Notes.Append("Price adjusted", $"{newPrice}. {reason}");

        MarkAsUpdated(clock);
    }

    public bool ConflictsWith(UtcDateTime start, UtcDateTime end)
    {
        if (IsDeleted) return false;

        if (end <= start)
            throw new ValidationException("Invalid time range.");

        if (IsTerminal())
            return false;

        return start < EndAt && end > StartAt;
    }

    private bool IsTerminal()
        => Status is AppointmentStatus.Completed
            or AppointmentStatus.Canceled
            or AppointmentStatus.NoShow;

    private void EnsureNotTerminal()
    {
        if (IsTerminal())
        {
            throw new StateException("Appointment cannot be modified.");
        }
    }

    private static void EnsureStartIsAfterNow(UtcDateTime startAt, IClock clock)
    {
        if (startAt <= clock.Now)
            throw new ValidationException("Appointment start time must be in the future.");
    }
}
