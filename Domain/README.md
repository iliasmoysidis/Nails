# Domain Layer

This project contains the **core business domain** for an appointment-based booking system.  
It is implemented using **Domain-Driven Design (DDD)** principles and is **fully persistence-agnostic**.

The domain defines:

- Business rules
- Invariants
- Aggregates
- Value objects
- Policies

It has **no dependency on infrastructure concerns** such as EF Core, databases, or HTTP.

---

## Design Principles

The domain follows these rules strictly:

- **Aggregates protect invariants**
- **Value objects are immutable**
- **Entities have identity and lifecycle**
- **Services coordinate, entities decide**
- **Persistence is an implementation detail**
- **Time and money are explicit value objects**
- **No anemic models**

---

## High-Level Concepts

### Stores & Staff

- A **Store** represents a business location.
- A **Staff** aggregate manages:
    - Owners
    - Employees (professionals)
- Only owners may perform administrative actions.

---

### Catalog & Offerings

- Each store has **one StoreCatalog**.
- A catalog contains:
    - **Offerings** (services provided by the store)
    - **ServiceOfferings** (assignments between professionals and offerings)
- An offering:
    - Has a price, duration, and lifecycle
    - Can be soft-deleted
- A professional can only be assigned to an offering **once per store**.

---

### Calendars

There are **two distinct calendars**:

#### StoreCalendar

- Defines when the store is open.
- Supports:
    - Weekly recurring working days
    - Date-specific exceptions
- Used to validate store availability.

#### StaffCalendar

- Defines when a professional is available **within a store**.
- A professional has **one calendar per store**.
- Supports:
    - Weekly working days
    - Date-specific exceptions
- Conflicts across stores are explicitly validated.

Calendars enforce:

- Non-overlapping time ranges
- Clear day-off semantics
- Explicit date handling

---

## Booking & Appointments

### Appointment

An **Appointment** is an entity with a full lifecycle:

- Pending confirmation
- Confirmed
- Completed
- Canceled
- No-show

Rules enforced:

- Appointments must start in the future
- Appointments cannot overlap
- Completed or canceled appointments are immutable
- Only authorized users can modify appointments
- Time-based restrictions apply (e.g. 24-hour rule)

---

### BookingContext

A **BookingContext** is a read-only snapshot used to validate booking rules.

It contains:

- StoreCatalog
- StoreCalendar
- StaffCalendar
- Staff
- Existing appointments

This allows all booking rules to be evaluated **without side effects**.

---

### Rule Engine

Booking validation is implemented via a **Rule Engine**:

- Each rule is isolated
- Rules are composable
- No rule performs persistence
- Rules fail fast via domain exceptions

Examples:

- Store must be open
- Professional must be available
- Offering must exist
- No overlapping appointments

---

## Authorization Policies

Authorization logic is implemented as **policies**, not services.

Example:

- `AppointmentAuthorizationPolicy` controls who can modify appointments and when.

This keeps:

- Security logic explicit
- Business logic testable
- Entities free from permission concerns

---

## Services

Domain services coordinate aggregates when logic:

- Spans multiple aggregates
- Does not belong naturally to a single entity

Examples:

- `BookingService`
- `StoreCatalogService`
- `StoreCalendarService`
- `StaffCalendarService`

Services:

- Load aggregates
- Enforce authorization
- Delegate decisions to entities
- Persist results via repositories

---

## Value Objects

The domain relies heavily on value objects to encode rules and meaning.

### Time

- `UtcDateTime` – UTC-only time representation
- `Duration` – constrained service duration
- `TimeRange` – validated time spans

### Money

- `Money` – currency-aware, safe arithmetic

### Identity

- `Email`
- `Phone`
- `FullName`
- `Address`
- `TaxIdentificationNumber`

### Calendar

- `WorkingDay`
- `CalendarException`

All value objects:

- Are immutable
- Validate on creation
- Cannot exist in an invalid state

---

## Repositories

Repositories are **interfaces only** and define aggregate boundaries.

Rules:

- Repositories return **fully valid aggregates**
- No partial aggregates
- No business logic in repositories

Examples:

- `IAppointmentRepository`
- `IStoreCatalogWriteRepository`
- `IStoreCalendarRepository`
- `IStaffCalendarRepository`

---

## Error Handling

All business rule violations are expressed via:

```csharp
DomainException
```
