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
    public TimeSpan Duration => EndAt - StartAt;

    public bool CanBeCanceled =>
        (IsPending || IsConfirmed)
        && (StartAt - DateTime.UtcNow).TotalHours >= 24;

    public bool CanBeRescheduled => CanBeCanceled;

    private Appointment()
    {
        Status = AppointmentStatus.PendingConfirmation;
    }

    public static Appointment Create(int userId, int professionalId, int serviceId, int storeId, decimal price, DateTime startAt, DateTime endAt, string? notes = null)
    {
        ValidateAppointmentInfo(price, startAt, endAt, notes);

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
        if (hoursTillStart < 24 && hoursTillStart > 0)
        {
            throw new DomainException("Cannot cancel appointments less than 24 hours before start time. Please contact the store directly.");
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

    public void Reschedule(DateTime newStartAt)
    {
        if (Status == AppointmentStatus.Completed)
        {
            throw new DomainException("Cannot reschedule a completed appointment.");
        }

        if (Status == AppointmentStatus.Canceled)
        {
            throw new DomainException("Cannot reschedule a canceled appointment. Create a new one instead.");
        }

        if (Status == AppointmentStatus.NoShow)
        {
            throw new DomainException("Cannot reschedule a no-show appointment. Create a new one instead.");
        }

        if (newStartAt <= DateTime.UtcNow)
        {
            throw new DomainException("Cannot reschedule to a time in the past.");
        }

        var hoursTillCurrentStart = (StartAt - DateTime.UtcNow).TotalHours;
        if (hoursTillCurrentStart < 24 && hoursTillCurrentStart > 0)
        {
            throw new DomainException("Cannot reschedule appointments less than 24 hours before start time. Please contact the store directly.");
        }

        StartAt = newStartAt;
        EndAt = newStartAt.Add(Duration);
        MarkAsUpdated();
    }

    public void UpdateNotes(string? notes)
    {
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

    public static void ValidateAppointmentInfo(Decimal price, DateTime startAt, DateTime endAt, string? notes = null)
    {
        if (startAt <= DateTime.UtcNow)
        {
            throw new DomainException("Cannot book appointments in the past.");
        }

        if (startAt >= endAt)
        {
            throw new DomainException("Start time must be before end time.");
        }

        if (price < 0)
        {
            throw new DomainException("Price cannot be negative.");
        }

        if (notes != null && notes.Length > 500)
        {
            throw new DomainException("Notes cannot be more than 500 characters.");
        }

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
