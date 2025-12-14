using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Appointment : HistoricEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int ServiceId { get; private set; }
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public decimal BookedPrice { get; private set; }
    public string? Notes { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public DateTime? CanceledAt { get; private set; }


    public bool IsPending => Status == AppointmentStatus.PendingConfirmation;
    public bool IsConfirmed => Status == AppointmentStatus.Confirmed;
    public bool IsCompleted => Status == AppointmentStatus.Completed;
    public bool IsCanceled => Status == AppointmentStatus.Canceled;
    public bool IsNoShow => Status == AppointmentStatus.NoShow;
    public bool IsUpComing => Status == AppointmentStatus.Confirmed && StartAt > DateTime.UtcNow;
    public bool IsPast => EndAt < DateTime.UtcNow;
    public bool IsInProgress => Status == AppointmentStatus.Confirmed && StartAt <= DateTime.UtcNow && EndAt > DateTime.UtcNow;

    private Appointment()
    {
        Status = AppointmentStatus.PendingConfirmation;
    }

    public static Appointment Create(int userId, int professionalId, int serviceId, int storeId, decimal price, DateTime startAt, DateTime endAt, string? notes = null)
    {
        ValidateAppointmentInfo(price, notes);
        ValidateTimeRange(startAt, endAt);
        ValidateChronology(startAt, endAt);
        ValidateStartIsInFuture(startAt);

        return new Appointment
        {
            UserId = userId,
            ServiceId = serviceId,
            ProfessionalId = professionalId,
            StoreId = storeId,
            BookedPrice = price,
            StartAt = startAt,
            EndAt = endAt,
            Notes = notes?.Trim(),
            Status = AppointmentStatus.PendingConfirmation,
        };
    }

    public void Confirm()
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot confirm deleted appointment.");
        }

        if (Status != AppointmentStatus.PendingConfirmation)
        {
            throw new DomainException($"Cannot confirm appointment. Current status is {Status}. Only pending appointments can be confirmed.");
        }

        if (StartAt <= DateTime.UtcNow)
        {
            throw new DomainException("Cannot confirm appointments in the past.");
        }

        Status = AppointmentStatus.Confirmed;
        MarkAsUpdated();
    }

    public void Cancel(string? reason = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot cancel deleted appointment.");
        }

        if (Status == AppointmentStatus.Completed)
        {
            throw new DomainException("Cannot cancel a completed appointment.");
        }

        if (Status == AppointmentStatus.Canceled)
        {
            throw new DomainException("Appointment is already canceled.");
        }

        if (Status == AppointmentStatus.NoShow)
        {
            throw new DomainException("Cannot cancel a no-show appointment.");
        }

        var hoursTillStart = (StartAt - DateTime.UtcNow).TotalHours;
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
        CanceledAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void Complete()
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot complete deleted appointment.");
        }

        if (Status != AppointmentStatus.Confirmed)
        {
            throw new DomainException($"Cannot complete appointment. Current status is {Status}. Only confirmed appointments can be completed.");
        }

        if (DateTime.UtcNow < EndAt)
        {
            throw new DomainException("Cannot complete appointments before its end time.");
        }

        Status = AppointmentStatus.Completed;
        MarkAsUpdated();
    }

    public void MarkAsNoShow()
    {
        if (Status != AppointmentStatus.Confirmed)
        {
            throw new DomainException($"Cannot mark as no-show. Current status is {Status}. Only confirmed appointments can be marked as no-show.");
        }

        if (DateTime.UtcNow < StartAt)
        {
            throw new DomainException("Cannot mark as no-show before appointment start time.");
        }

        Status = AppointmentStatus.NoShow;
        MarkAsUpdated();
    }

    public void Reschedule(DateTime startAt, DateTime endAt)
    {
        if (IsDeleted || IsCompleted || IsCanceled || IsNoShow)
            throw new DomainException("Appointment cannot be rescheduled.");

        if (StartAt <= DateTime.UtcNow)
        {
            throw new DomainException("Cannot reschedule an appointment that has already started.");
        }

        ValidateTimeRange(startAt, endAt);
        ValidateChronology(startAt, endAt);
        ValidateStartIsInFuture(startAt);

        this.StartAt = startAt;
        this.EndAt = endAt;
        MarkAsUpdated();
    }

    public void UpdateNotes(string? notes)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot update the notes of deleted appointment.");
        }

        if (Status == AppointmentStatus.Completed)
        {
            throw new DomainException("Cannot update notes for completed appointments.");
        }

        if (Status == AppointmentStatus.Canceled)
        {
            throw new DomainException("Cannot update notes for canceled appointments.");
        }

        if (Status == AppointmentStatus.NoShow)
        {
            throw new DomainException("Cannot update notes for no-show appointments.");
        }

        Notes = notes?.Trim();
        MarkAsUpdated();
    }

    public void AdjustPrice(decimal newPrice, string reason)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot adjust the price of a deleted appointment.");
        }

        if (Status == AppointmentStatus.Completed)
        {
            throw new DomainException("Cannot adjust price for completed appointments.");
        }

        if (Status == AppointmentStatus.Canceled)
        {
            throw new DomainException("Cannot adjust price for canceled appointments.");
        }

        if (Status == AppointmentStatus.NoShow)
        {
            throw new DomainException("Cannot adjust price for no-show appointments.");
        }

        if (newPrice < 0)
        {
            throw new DomainException("Price cannot be negative.");
        }

        BookedPrice = newPrice;

        var adjustmentNote = $"Price adjusted to {newPrice:C}. Reason: {reason}";
        Notes = string.IsNullOrWhiteSpace(Notes)
            ? adjustmentNote
            : $"{Notes}\n{adjustmentNote}";

        MarkAsUpdated();
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

    private static void ValidateChronology(DateTime startAt, DateTime endAt)
    {
        if (startAt >= endAt)
        {
            throw new DomainException("End time must be after start time.");
        }
    }

    private static void ValidateStartIsInFuture(DateTime startAt)
    {
        if (startAt <= DateTime.UtcNow)
        {
            throw new DomainException("Cannot schedule appointments in the past.");
        }
    }

    private static void ValidateTimeRange(DateTime startAt, DateTime endAt)
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
}
