using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
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
    public UtcDateTime EndAt { get; private set; }
    public decimal BookedPrice { get; private set; }
    public string? Notes { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public UtcDateTime? CanceledAt { get; private set; }


    public bool IsPending => Status == AppointmentStatus.PendingConfirmation;
    public bool IsConfirmed => Status == AppointmentStatus.Confirmed;
    public bool IsCompleted => Status == AppointmentStatus.Completed;
    public bool IsCanceled => Status == AppointmentStatus.Canceled;
    public bool IsNoShow => Status == AppointmentStatus.NoShow;
    public bool IsUpComing(UtcDateTime now) => Status == AppointmentStatus.Confirmed && StartAt > now;
    public bool IsPast(UtcDateTime now) => EndAt < now;
    public bool IsInProgress(UtcDateTime now) => Status == AppointmentStatus.Confirmed && StartAt <= now && EndAt > now;

    private Appointment()
    {
        Status = AppointmentStatus.PendingConfirmation;
    }

    public static Appointment Create(int userId, int professionalId, int offeringId, int storeId, decimal price, UtcDateTime startAt, UtcDateTime endAt, IClock clock, string? notes = null)
    {
        ValidateAppointmentInfo(price, notes);
        ValidateTimeRange(startAt, endAt);
        ValidateChronology(startAt, endAt);
        ValidateStartIsInFuture(clock, startAt);

        var appointment = new Appointment
        {
            UserId = userId,
            OfferingId = offeringId,
            ProfessionalId = professionalId,
            StoreId = storeId,
            BookedPrice = price,
            StartAt = startAt,
            EndAt = endAt,
            Notes = notes?.Trim(),
            Status = AppointmentStatus.PendingConfirmation,
        };

        appointment.MarkAsCreated(clock);

        return appointment;
    }

    public void Confirm(IClock clock)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot confirm deleted appointment.");
        }

        if (Status != AppointmentStatus.PendingConfirmation)
        {
            throw new DomainException($"Cannot confirm appointment. Current status is {Status}. Only pending appointments can be confirmed.");
        }

        if (StartAt <= clock.Now)
        {
            throw new DomainException("Cannot confirm appointments in the past.");
        }

        Status = AppointmentStatus.Confirmed;
        MarkAsUpdated(clock);
    }

    public void Cancel(IClock clock, string? reason = null)
    {
        EnsureIsMutable();

        var hoursTillStart = (StartAt - clock.Now).TotalHours;
        if (hoursTillStart <= 0)
        {
            throw new DomainException("Cannot cancel an ongoing appointment.");
        }

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
        if (IsDeleted)
        {
            throw new DomainException("Cannot complete deleted appointment.");
        }

        if (Status != AppointmentStatus.Confirmed)
        {
            throw new DomainException($"Cannot complete appointment. Current status is {Status}. Only confirmed appointments can be completed.");
        }

        if (clock.Now < EndAt)
        {
            throw new DomainException("Cannot complete appointments before its end time.");
        }

        Status = AppointmentStatus.Completed;
        MarkAsUpdated(clock);
    }

    public void MarkAsNoShow(IClock clock)
    {
        if (Status != AppointmentStatus.Confirmed)
        {
            throw new DomainException($"Cannot mark as no-show. Current status is {Status}. Only confirmed appointments can be marked as no-show.");
        }

        if (clock.Now < StartAt)
        {
            throw new DomainException("Cannot mark as no-show before appointment start time.");
        }

        Status = AppointmentStatus.NoShow;
        MarkAsUpdated(clock);
    }

    public void Reschedule(UtcDateTime startAt, UtcDateTime endAt, IClock clock)
    {
        EnsureIsMutable();

        if (StartAt <= clock.Now)
        {
            throw new DomainException("Cannot reschedule an appointment that has already started.");
        }

        ValidateTimeRange(startAt, endAt);
        ValidateChronology(startAt, endAt);
        ValidateStartIsInFuture(clock, startAt);

        this.StartAt = startAt;
        this.EndAt = endAt;
        MarkAsUpdated(clock);
    }

    public void UpdateNotes(IClock clock, string? notes)
    {
        EnsureIsMutable();

        Notes = notes?.Trim();
        MarkAsUpdated(clock);
    }

    public void AdjustPrice(IClock clock, decimal newPrice, string reason)
    {
        EnsureIsMutable();

        if (newPrice < 0)
        {
            throw new DomainException("Price cannot be negative.");
        }

        BookedPrice = newPrice;

        var adjustmentNote = $"Price adjusted to {newPrice:C}. Reason: {reason}";
        Notes = string.IsNullOrWhiteSpace(Notes)
            ? adjustmentNote
            : $"{Notes}\n{adjustmentNote}";

        MarkAsUpdated(clock);
    }

    private static void ValidateAppointmentInfo(Decimal price, string? notes = null)
    {
        if (price < 0)
        {
            throw new DomainException("Price cannot be negative.");
        }

        if (notes != null && notes.Length > 500)
        {
            throw new DomainException("Notes cannot be more than 500 characters.");
        }
    }

    private static void ValidateChronology(UtcDateTime startAt, UtcDateTime endAt)
    {
        if (startAt >= endAt)
        {
            throw new DomainException("End time must be after start time.");
        }
    }

    private static void ValidateStartIsInFuture(IClock clock, UtcDateTime startAt)
    {
        if (startAt <= clock.Now)
        {
            throw new DomainException("Cannot schedule appointments in the past.");
        }
    }

    private static void ValidateTimeRange(UtcDateTime startAt, UtcDateTime endAt)
    {
        var duration = endAt - startAt;
        if (duration > TimeSpan.FromHours(8))
        {
            throw new DomainException("Appointment duration cannot exceed 8 hours.");
        }

        if (duration.TotalMinutes % 15 != 0)
        {
            throw new DomainException("Appointment duration must be in 15-minute increments.");
        }

        if (startAt.Minute % 15 != 0 || startAt.Second != 0)
        {
            throw new DomainException("Appointments must start on 15-minute intervals.");
        }
    }

    private void EnsureIsMutable()
    {
        if (IsDeleted || IsCompleted || IsCanceled || IsNoShow)
        {
            throw new DomainException("Appointment cannot be modified.");
        }
    }

    public bool ConflictsWith(UtcDateTime start, UtcDateTime end)
    {
        if (IsDeleted) return false;

        if (Status is AppointmentStatus.Canceled or AppointmentStatus.Completed or AppointmentStatus.NoShow)
            return false;

        return start < EndAt && end > StartAt;
    }
}
