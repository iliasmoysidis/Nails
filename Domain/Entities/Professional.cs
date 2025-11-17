using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Professional : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;

    private readonly List<StoreOwner> _ownedStores = new();
    public IReadOnlyCollection<StoreOwner> OwnedStores => _ownedStores.AsReadOnly();

    private readonly List<StoreProfessional> _workplaces = new();
    public IReadOnlyCollection<StoreProfessional> Workplaces => _workplaces.AsReadOnly();

    private readonly List<ProfessionalService> _providedServices = new();
    public IReadOnlyCollection<ProfessionalService> ProvidedServices => _providedServices.AsReadOnly();

    private readonly List<ProfessionalSchedule> _workSchedules = new();
    public IReadOnlyCollection<ProfessionalSchedule> WorkSchedules => _workSchedules.AsReadOnly();

    private readonly List<ProfessionalTimeOff> _timeOffs = new();
    public IReadOnlyCollection<ProfessionalTimeOff> TimeOffs => _timeOffs.AsReadOnly();

    private Professional() { }

    public static Professional Create(string name, string surname, string email, string phone, string taxIdNumber)
    {
        ValidatePersonalInfo(name, surname, email, phone, taxIdNumber);

        var professional = new Professional
        {
            Name = name.Trim(),
            Surname = surname.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            Phone = phone.Trim(),
            TaxIdNumber = taxIdNumber.Trim()
        };

        return professional;
    }

    public void UpdatePersonalInfo(string? name = null, string? surname = null, string? phone = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot update inactive professional.");
        }

        var hasChanges = false;
        if (name != null && name != Name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Name cannot be empty.");
            }

            if (name.Length > 100)
            {
                throw new DomainException("Name cannot exceed 100 characters.");
            }

            Name = name.Trim();
            hasChanges = true;
        }

        if (surname != null && surname != Surname)
        {
            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new DomainException("Surname cannot be empty.");
            }

            if (surname.Length > 100)
            {
                throw new DomainException("Surname cannot exceed 100 characters.");
            }

            Surname = surname.Trim();
            hasChanges = true;
        }

        if (phone != null && phone != Phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new DomainException("Phone cannot be empty.");
            }

            if (phone.Length > 20)
            {
                throw new DomainException("Phone cannot exceed 20 characters.");
            }

            Phone = phone.Trim();
            hasChanges = true;
        }

        if (hasChanges) MarkAsUpdated();
    }

    public void AddWorkplace(Store store)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add workplace to inactive professional.");
        }

        if (store.IsDeleted)
        {
            throw new DomainException("Cannot add professional to inactive store.");
        }

        var existingActiveWorkplace = _workplaces.FirstOrDefault(w => w.StoreId == store.Id);

        if (existingActiveWorkplace != null)
        {
            throw new DomainException("Professional already works at this store.");
        }

        var workplace = StoreProfessional.Create(store.Id, Id);
        _workplaces.Add(workplace);

        MarkAsUpdated();
    }

    public void RemoveWorkplace(int storeId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove workplace from inactive professional.");
        }

        var workplace = _workplaces.FirstOrDefault(w => w.StoreId == storeId);

        if (workplace == null)
        {
            throw new DomainException("Professional does not work at this store.");
        }

        _workSchedules.RemoveAll(s => s.StoreId == storeId);
        _timeOffs.RemoveAll(t => t.StoreId == storeId);
        _workplaces.Remove(workplace);

        MarkAsUpdated();
    }

    public bool WorksAtStore(int storeId)
    {
        return !IsDeleted && _workplaces.Any(w => w.StoreId == storeId);
    }

    public void AddService(Service service)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add service to inactive professional.");
        }

        if (service.IsDeleted)
        {
            throw new DomainException("Cannot add inactive service.");
        }

        var worksAtStore = WorksAtStore(service.StoreId);
        if (!worksAtStore)
        {
            throw new DomainException("Professional must work at the store where the service is provided.");
        }

        var alreadyProvides = _providedServices.Any(ps => ps.ServiceId == service.Id);
        if (alreadyProvides)
        {
            throw new DomainException("Professional already provides this service.");
        }

        var professionalService = ProfessionalService.Create(Id, service.Id);
        _providedServices.Add(professionalService);

        MarkAsUpdated();
    }

    public void RemoveService(int serviceId)
    {
        var professionalService = _providedServices.FirstOrDefault(ps => ps.ServiceId == serviceId);

        if (professionalService == null)
        {
            throw new DomainException("Professional does not provide this service.");
        }

        _providedServices.Remove(professionalService);
        MarkAsUpdated();
    }

    public bool ProvidesService(int serviceId)
    {
        return !IsDeleted && _providedServices.Any(ps => ps.ServiceId == serviceId);
    }

    public void AddSchedule(ProfessionalSchedule newSchedule)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add schedule to inactive professional.");
        }

        if (!newSchedule.IsActive)
        {
            throw new DomainException("Cannot add inactive schedule");
        }

        if (newSchedule.StartTime >= newSchedule.EndTime)
        {
            throw new DomainException("Schedule start time must be before end time.");
        }

        if (!WorksAtStore(newSchedule.StoreId))
        {
            throw new DomainException("Professional must work at the store to set schedule.");
        }

        var overlapping = _workSchedules.Any(s =>
            s.ProfessionalId == Id &&
            s.DayOfWeek == newSchedule.DayOfWeek &&
            s.IsActive &&
            newSchedule.StartTime < s.EndTime &&
            newSchedule.EndTime > s.StartTime);

        if (overlapping)
        {
            throw new DomainException("Schedule overlaps with an existing schedule.");
        }

        _workSchedules.Add(newSchedule);
        MarkAsUpdated();
    }

    public void RemoveSchedule(int scheduleId)
    {
        var schedule = _workSchedules.FirstOrDefault(s => s.Id == scheduleId);

        if (schedule == null)
        {
            throw new DomainException("Schedule not found.");
        }

        if (!schedule.IsActive)
        {
            throw new DomainException("Schedule is already inactive.");
        }

        schedule.Deactivate();
        MarkAsUpdated();
    }

    public IEnumerable<ProfessionalSchedule> GetActiveSchedulesForStore(int storeId)
    {
        return _workSchedules
            .Where(s => s.StoreId == storeId && s.IsActive)
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.StartTime);
    }

    public void RequestTimeOff(DateTime startAt, DateTime endAt, TimeOffType? type = null, string? reason = null, int? storeId = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot request time off for inactive professional.");
        }

        if (startAt >= endAt)
        {
            throw new DomainException("Start date must be before end date.");
        }

        if (startAt < DateTime.UtcNow.Date)
        {
            throw new DomainException("Cannot request time off in the past.");
        }

        var hasOverlap = _timeOffs.Any(t => startAt < t.EndAt && endAt > t.StartAt);

        if (hasOverlap)
        {
            throw new DomainException("Time off request overlaps with existing time off.");
        }

        if (storeId.HasValue && !WorksAtStore(storeId.Value))
        {
            throw new DomainException("Professional does not work at this store.");
        }

        var timeOff = ProfessionalTimeOff.Create(
            Id, startAt, endAt, type, reason, storeId
        );

        _timeOffs.Add(timeOff);
        MarkAsUpdated();
    }

    public void CancelTimeOff(int timeOffId)
    {
        var timeOff = _timeOffs.FirstOrDefault(t => t.Id == timeOffId);

        if (timeOff == null)
        {
            throw new DomainException("Time off request not found.");
        }

        if (timeOff.StartAt < DateTime.UtcNow)
        {
            throw new DomainException("Cannot cancel time off that has already started.");
        }

        _timeOffs.Remove(timeOff);
        MarkAsUpdated();
    }

    public bool IsAvailableAt(DateTime dateTime, int storeId)
    {
        if (IsDeleted)
        {
            return false;
        }

        if (!WorksAtStore(storeId))
        {
            return false;
        }

        var dayOfWeek = dateTime.DayOfWeek;
        var timeOfDay = dateTime.TimeOfDay;

        var schedule = _workSchedules.FirstOrDefault(s =>
            s.StoreId == storeId &&
            s.DayOfWeek == dayOfWeek &&
            s.IsActive);

        if (schedule == null)
        {
            return false;
        }

        if (timeOfDay < schedule.StartTime || timeOfDay >= schedule.EndTime)
        {
            return false;
        }

        var hasTimeOff = _timeOffs.Any(t =>
            dateTime >= t.StartAt &&
            dateTime < t.EndAt &&
            (t.StoreId == null || t.StoreId == storeId));

        if (hasTimeOff)
        {
            return false;
        }

        return true;
    }

    public bool IsOwnerOf(int storeId)
    {
        return _ownedStores.Any(m => m.StoreId == storeId && m.ProfessionalId == Id);
    }

    public string FullName => $"{Name} {Surname}";

    public IEnumerable<int> GetActiveStoreIds()
    {
        return _workplaces
            .Select(w => w.StoreId)
            .Distinct();
    }

    public IEnumerable<int> GetProvidedServiceIds()
    {
        return _providedServices.Select(ps => ps.ServiceId);
    }

    public void Deactivate(string? reason = null)
    {
        if (IsDeleted)
        {
            throw new DomainException("Professional is already deactivated.");
        }

        SoftDelete();
    }

    private static void ValidatePersonalInfo(string name, string surname, string email, string phone, string taxIdNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required.");
        }

        if (name.Length > 100)
        {
            throw new DomainException("Name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(surname))
        {
            throw new DomainException("Surname is required.");
        }

        if (surname.Length > 100)
        {
            throw new DomainException("Surname cannot exceed 100 characters.");
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