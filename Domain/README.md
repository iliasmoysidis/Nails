# Domain Aggregates Overview

This document lists all aggregates, their roots, and their internal entities.

---

## **Aggregate: User**

**Root:** `User`

---

## **Aggregate: StoreEmployee**

**Root:** `StoreStaff`  
**Entities:**

-   `StoreOwner`
-   `StoreEmployee`

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

-   `StoreEmployeeSchedule`
-   `StoreEmployeeScheduleSpecial`

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
