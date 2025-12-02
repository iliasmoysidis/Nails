# Domain Aggregates Overview

This document lists all aggregates, their roots, and their internal entities.

---

## **Aggregate: User**

**Root:** `User`

---

## **Aggregate: Employee**

**Root:** `Staff`  
**Entities:**

-   `StoreOwner`
-   `Employee`

---

## **Aggregate: StoreSchedule**

**Root:** `StoreCalendar`  
**Entities:**

-   `StoreSchedule`
-   `StoreScheduleSpecial`

---

## **Aggregate: StaffSchedule**

**Root:** `StaffCalendar`  
**Entities:**

-   `EmployeeSchedule`
-   `EmployeeScheduleSpecial`

---

## **Aggregate: StoreServiceCatalog**

**Root:** `StoreCatalog`  
**Entities:**

-   `Service`
-   `StaffService`

---

## **Aggregate: Appointment**

**Root:** `Appointment`

---
