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

    public User User { get; private set; } = null!;
    public Professional Professional { get; private set; } = null!;
    public Service Service { get; private set; } = null!;
    public Store Store { get; private set; } = null!;

    private Appointment()
    {
        Status = AppointmentStatus.PendingConfirmation;
    }

    public static Appointment Schedule(User user, Professional professional, Service service, Store store, DateTime startAt, string? notes = null)
    {
        var endAt = startAt.Add(service.Duration);

        var appointment = new Appointment
        {
            UserId = user.Id,
            ProfessionalId = professional.Id,
            ServiceId = service.Id,
            StoreId = store.Id,
            StartAt = startAt,
            EndAt = endAt,
            BookedPrice = service.Price,
            Notes = notes?.Trim(),
            Status = AppointmentStatus.PendingConfirmation
        };

        appointment.User = user;
        appointment.Professional = professional;
        appointment.Service = service;
        appointment.Store = store;

        return appointment;
    }

    public void Confirmm()
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

    public void Reschedule(DateTime newStartAt, Service? newService = null)
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

        if (newService != null && newService.Id != ServiceId)
        {
            ServiceId = newService.Id;
            Service = newService;
            BookedPrice = newService.Price;
            StartAt = newStartAt;
            EndAt = newStartAt.Add(newService.Duration);
        }
        else
        {
            var duration = EndAt - StartAt;
            StartAt = newStartAt;
            EndAt = newStartAt.Add(duration);
        }

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

    public static void ValidateSchedulingRules(User user, Professional professional, Service service, Store store, DateTime startAt)
    {
        if (user.IsDeleted)
        {
            throw new DomainException("User account is not active.");
        }

        if (professional.IsDeleted)
        {
            throw new DomainException("Professional is not available.");
        }

        if (service.IsDeleted)
        {
            throw new DomainException("Service is no longer available.");
        }

        if (store.IsDeleted)
        {
            throw new DomainException("Store is not active.");
        }

        if (service.StoreId != store.Id)
        {
            throw new DomainException("Service does not belong to this store.");
        }

        if (startAt <= DateTime.UtcNow)
        {
            throw new DomainException("Cannot book appointments in the past.");
        }

        var hoursInAdvance = (startAt - DateTime.UtcNow).TotalHours;
        if (hoursInAdvance < 2)
        {
            throw new DomainException("Appointments must be booked at least 2 hours in advance.");
        }

        var daysInAdvance = (startAt - DateTime.UtcNow).TotalDays;
        if (daysInAdvance > 45)
        {
            throw new DomainException("Cannot book appointments more than 45 days in advance.");
        }

        if (startAt.Minute % 15 != 0 || startAt.Second != 0)
        {
            throw new DomainException("Appointments must start on 15 minute intervals.");
        }
    }

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
        (Status == AppointmentStatus.PendingConfirmation || Status == AppointmentStatus.Confirmed)
        && (StartAt - DateTime.Now).TotalHours >= 24;

    public bool CanBeRescheduled =>
        (Status == AppointmentStatus.PendingConfirmation || Status == AppointmentStatus.Confirmed)
        && (StartAt - DateTime.Now).TotalHours >= 24;

    public void ValidateNoTimeConflict(IEnumerable<Appointment> professionalAppointments)
    {
        foreach (var existing in professionalAppointments)
        {
            if (existing.Id == Id) continue;

            if (existing.Status == AppointmentStatus.Canceled || existing.Status == AppointmentStatus.NoShow) continue;

            if (StartAt < existing.EndAt && EndAt > existing.StartAt)
            {
                throw new DomainException($"Professional is not available at this time. Conflicts with appointment from {existing.StartAt:g} to {existing.EndAt:g}.");
            }
        }
    }
}
