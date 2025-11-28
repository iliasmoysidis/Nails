using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class Store : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    private readonly List<StoreOwner> _owners = new();
    public IReadOnlyCollection<StoreOwner> Owners => _owners.AsReadOnly();

    private readonly List<StoreProfessional> _staff = new();
    public IReadOnlyCollection<StoreProfessional> Staff => _staff.AsReadOnly();

    private readonly List<StoreProfessionalSchedule> _staffSchedules = new();
    public IReadOnlyCollection<StoreProfessionalSchedule> StaffSchedules => _staffSchedules.AsReadOnly();

    private readonly List<StoreProfessionalException> _staffExceptions = new();
    public IReadOnlyCollection<StoreProfessionalException> StaffExceptions => _staffExceptions.AsReadOnly();

    private readonly List<ProfessionalService> _staffServices = new();
    public IReadOnlyCollection<ProfessionalService> StaffServices => _staffServices.AsReadOnly();

    private readonly List<StoreSchedule> _storeSchedules = new();
    public IReadOnlyCollection<StoreSchedule> StoreSchedules => _storeSchedules.AsReadOnly();

    private readonly List<StoreException> _exceptions = new();
    public IReadOnlyCollection<StoreException> Exceptions => _exceptions.AsReadOnly();

    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

    private readonly List<Appointment> _appointments = new();
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private Store() { }

    public bool IsOwner(int professionalId)
    {
        return _owners.Any(o => o.ProfessionalId == professionalId);
    }

    public bool IsStaff(int professionalId)
    {
        return _staff.Any(s => s.ProfessionalId == professionalId);
    }

    public bool ServiceIsProvidedByProfessional(int professionalId, int serviceId)
    {
        return _staffServices.Any(ps => ps.ProfessionalId == professionalId && ps.ServiceId == serviceId);
    }

    public bool ServiceIsProvidedByTheStore(int serviceId)
    {
        return _services.Any(s => s.Id == serviceId && !s.IsDeleted);
    }

    public static Store Create(string name, string address, string taxIdNumber, string email, string phone)
    {
        ValidateStoreInfo(name, address, taxIdNumber, email, phone);
        return new Store
        {
            Name = name,
            Address = address,
            TaxIdNumber = taxIdNumber,
            Email = email,
            Phone = phone
        };
    }

    public StoreOwner AddOwner(int ownerId, int prospectiveOwnerId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add owner to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add an owner.");
        }

        if (IsOwner(prospectiveOwnerId))
        {
            throw new DomainException("This professional is already an owner of this store.");
        }

        var newOwner = StoreOwner.Create(Id, prospectiveOwnerId);
        _owners.Add(newOwner);
        MarkAsUpdated();

        return newOwner;
    }

    public void RemoveOwner(int ownerId, int toBeRemovedOwnerId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove owner from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove an owner.");
        }

        var toBeRemovedOwner = _owners.FirstOrDefault(s => s.ProfessionalId == toBeRemovedOwnerId);

        if (toBeRemovedOwner == null)
        {
            throw new DomainException("Professional is not an owner of this store.");
        }

        if (_owners.Count() == 1)
        {
            throw new DomainException("Cannot remove last owner.");
        }

        _owners.Remove(toBeRemovedOwner);
        MarkAsUpdated();
    }

    public Service AddService(int ownerId, string name, decimal price, TimeSpan duration, string? description = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add service to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add a service.");
        }

        var service = Service.Create(Id, name, price, duration, description);
        _services.Add(service);
        MarkAsUpdated();

        return service;
    }

    public void RemoveService(int ownerId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove service from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove a service.");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Could not find service.");
        }

        service.Deactivate();
        MarkAsUpdated();
    }

    public ProfessionalService AssignStaffToService(int ownerId, int professionalId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot assign services in an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can assign a staff to a service.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot assign services to a professional who does not work at the store.");
        }

        if (ServiceIsProvidedByProfessional(professionalId, serviceId))
        {
            throw new DomainException("Service is already offered by this professional");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Could not find service.");
        }

        var professionalService = ProfessionalService.Create(professionalId, serviceId);
        _staffServices.Add(professionalService);
        MarkAsUpdated();

        return professionalService;
    }

    public void UnassignStaffFromService(int ownerId, int professionalId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot unassign services in an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can unassign a staff from a service.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot unassign services from professionals who do not work at the store.");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Could not find service.");
        }

        var professionalService = _staffServices.FirstOrDefault(
            ps => ps.ProfessionalId == professionalId &&
            ps.ServiceId == serviceId);

        if (professionalService == null)
        {
            throw new DomainException("Service is not offered by the professional.");
        }

        _staffServices.Remove(professionalService);
        MarkAsUpdated();
    }

    public Appointment BookAppointment(int userId, int professionalId, int serviceId, DateTime startAt, string? notes = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot book appointments at an inactive store.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Professional does not work at this store.");
        }

        if (!ServiceIsProvidedByTheStore(serviceId))
        {
            throw new DomainException("The service is not offered by this store.");
        }

        if (!ServiceIsProvidedByProfessional(professionalId, serviceId))
        {
            throw new DomainException("The professional is not offering this service.");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Could not find service.");
        }

        if (!IsOpenAt(startAt))
        {
            throw new DomainException("Store is closed at the requested time.");
        }

        var endAt = startAt.Add(service.Duration);

        if (!IsOpenAt(endAt.AddMinutes(-1)))
        {
            throw new DomainException("Appointment extends beyond store closing time.");
        }

        if (!IsProfessionalAvailable(professionalId, startAt, endAt))
        {
            throw new DomainException("Professional is not available at the requested time.");
        }

        if (HasAppointmentConflict(professionalId, startAt, endAt))
        {
            throw new DomainException("Professional already has an appointment at this time.");
        }

        var appointment = Appointment.Create(userId, professionalId, serviceId, Id, service.Price, startAt, endAt, notes);
        _appointments.Add(appointment);
        MarkAsUpdated();

        return appointment;
    }

    public void RescheduleAppointment(int appointmentId, DateTime newStartAt)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot reschedule appointments at an inactive store.");
        }

        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        var service = _services.FirstOrDefault(s => s.Id == appointment.ServiceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Service not found.");
        }

        var newEndAt = newStartAt.Add(service.Duration);

        if (!IsOpenAt(newStartAt))
        {
            throw new DomainException("Store is closed at the requested time.");
        }

        if (!IsOpenAt(newEndAt.AddMinutes(-1)))
        {
            throw new DomainException("Appointment extends beyong store closing time.");
        }

        if (!IsProfessionalAvailable(appointment.ProfessionalId, newStartAt, newEndAt))
        {
            throw new DomainException("Professional is not available at the requested time.");
        }

        if (HasAppointmentConflict(appointment.ProfessionalId, newStartAt, newEndAt))
        {
            throw new DomainException("Professional already has an appointment at this time.");
        }

        appointment.Reschedule(newStartAt);
        MarkAsUpdated();
    }

    public void CancelAppointment(int appointmentId, string? reason = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot cancel appointments at an inactive store.");
        }

        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        appointment.Cancel(reason);
        MarkAsUpdated();
    }

    public void ConfirmAppointment(int ownerId, int appointmentId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot confirm appointments at an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can confirm appointments.");
        }

        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        appointment.Confirm();
        MarkAsUpdated();
    }

    public void CompleteAppointment(int ownerId, int appointmentId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot complete appointments at an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can complete appointments.");
        }

        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        appointment.Complete();
        MarkAsUpdated();
    }

    public void MarkAppointmentAsNoShow(int ownerId, int appointmentId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot mark appointments as no show at an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can mark an appointment as no show.");
        }

        var appointment = _appointments.FirstOrDefault(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new DomainException("Appointment not found.");
        }

        appointment.MarkAsNoShow();
        MarkAsUpdated();
    }

    private bool IsOpenAt(DateTime date)
    {
        var dateExceptions = _exceptions.Where(e => e.Date.Date == date.Date).ToList();

        if (dateExceptions.Any())
        {
            return dateExceptions.Any(
                e => !e.IsFullDayClosed &&
                e.OpenTime.HasValue &&
                e.CloseTime.HasValue &&
                date.TimeOfDay >= e.OpenTime.Value &&
                date.TimeOfDay < e.CloseTime.Value
            );
        }

        var dayHours = _storeSchedules.Where(h => h.Day == date.DayOfWeek).ToList();

        return dayHours.Any(
            h => !h.IsFullDayClosed &&
            h.OpenTime.HasValue &&
            h.CloseTime.HasValue &&
            date.TimeOfDay >= h.OpenTime.Value &&
            date.TimeOfDay < h.CloseTime.Value);
    }

    private bool IsOpenOn(DayOfWeek day)
    {
        return !_storeSchedules.Any(h => h.Day == day && h.IsFullDayClosed);
    }

    private bool IsWithinStoreHours(DayOfWeek day, TimeSpan? startTime, TimeSpan? endTime)
    {
        return _storeSchedules.Any(h => h.Day == day && h.OpenTime <= startTime && h.CloseTime >= endTime);
    }

    private static void ValidateStoreInfo(string name, string address, string taxIdNumber, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required.");
        }

        if (name.Length > 100)
        {
            throw new DomainException("Name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new DomainException("Address is required.");
        }

        if (address.Length > 200)
        {
            throw new DomainException("Address cannot exceed 200 characters.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required.");
        }

        if (!IsValidEmail(email))
        {
            throw new DomainException("Invalid email format.");
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new DomainException("Phone is required.");
        }

        if (phone.Length > 20)
        {
            throw new DomainException("Phone cannot exceed 20 characters.");
        }

        if (string.IsNullOrWhiteSpace(taxIdNumber))
        {
            throw new DomainException("Tax ID number is required.");
        }

        if (taxIdNumber.Length > 50)
        {
            throw new DomainException("Tax ID number cannot exceed 50 characters.");
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
